using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JLGames.Infra.Crypto.Asymmetric;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Home
{
    public class RabbitHomeClient
    {
        private readonly string m_HomeUrl;
        private readonly bool m_UsePost;
        private readonly TimeSpan m_Timeout;

        private QueryRouteInfo m_QueryInfo;
        private IRsaPublicCipher m_PubRsaCipher;

        public RabbitHomeClient(string homeUrl, bool usePost)
        {
            m_HomeUrl = homeUrl;
            m_UsePost = usePost;
            m_Timeout = TimeSpan.FromSeconds(100);
        }

        public RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)
        {
            m_HomeUrl = homeUrl;
            m_UsePost = usePost;
            m_Timeout = timeout;
        }

        /// <summary>
        /// 设置RSA公钥信息
        /// </summary>
        /// <param name="pub"></param>
        public void SetPublicRsa(IRsaPublicCipher pub)
        {
            m_PubRsaCipher = pub;
        }

        /// <summary>
        /// 设置RSA公钥信息
        /// </summary>
        /// <param name="pubKey"></param>
        public void SetPublicRsa(RSA pubKey)
        {
            SetPublicRsa(new RsaPublicCipher(pubKey));
        }

        /// <summary>
        /// 向 RabbitHome服务器 查询可用实例
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="isPemKey"></param>
        /// <param name="publicKeyPath"></param>
        /// <returns></returns>
        public Task<QueryResult> QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath)
        {
            IRsaPublicCipher pubCipher;
            if (isPemKey)
                pubCipher = RsaUtils.LoadPublicCipherX509(publicKeyPath);
            else
                pubCipher = RsaUtils.LoadPublicCipherPkcs1V15(publicKeyPath);
            if (null == pubCipher)
            {
                return Task.FromResult(new QueryResult { Ok = false, KeyError = true });
            }

            SetPublicRsa(pubCipher);
            return QueryFromHome(queryInfo);
        }

        /// <summary>
        /// 向 RabbitHome服务器 查询可用实例
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="publicCipher"></param>
        /// <returns></returns>
        public Task<QueryResult> QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)
        {
            SetPublicRsa(publicCipher);
            return QueryFromHome(queryInfo);
        }

        /// <summary>
        /// 向 RabbitHome服务器 查询可用实例
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public Task<QueryResult> QueryFromHome(QueryRouteInfo queryInfo)
        {
            if (null == queryInfo) return Task.FromResult(new QueryResult { Ok = false, ParamError = true });
            m_QueryInfo = queryInfo;
            var json = queryInfo.ToJsonString();
            var bs = Encoding.UTF8.GetBytes(json);
            if (null != m_PubRsaCipher)
            {
                bs = m_PubRsaCipher.Encrypt(bs);
            }

            return DoQuery(bs);
        }

        private Task<QueryResult> DoQuery(byte[] content)
        {
            if (m_UsePost)
                return DoPost(content);
            else
                return DoGet(content);
        }

        private async Task<QueryResult> DoGet(byte[] bytes)
        {
            var base64 = RabbitHomeDefaults.Base64Encoding.EncodeToString(bytes);
            var pattern = $"{RabbitHomeDefaults.HttpPatternRoute}?{RabbitHomeDefaults.HttpKeyQuery}={base64}";
            using (var proxy = new HttpClientProxy(m_HomeUrl, m_Timeout))
            {
                var result = await proxy.GetBytesAsync(pattern, m_Timeout);
                return HandleHomeResponse(result);
            }
        }

        private async Task<QueryResult> DoPost(byte[] bytes)
        {
            var base64 = RabbitHomeDefaults.Base64Encoding.EncodeToString(bytes);
            var pattern = $"{RabbitHomeDefaults.HttpPatternRoute}";
            var value = new Dictionary<string, string>
            {
                { RabbitHomeDefaults.HttpKeyQuery, base64 }
            };
            using (var proxy = new HttpClientProxy(m_HomeUrl, m_Timeout))
            {
                var result = await proxy.PostBytesAsync(pattern, value, m_Timeout);
                return HandleHomeResponse(result);
            }
        }

        private QueryResult HandleHomeResponse(HttpResult<byte[]> result)
        {
            if (result.Timeout)
                return new QueryResult { Ok = false, TimeOut = true };

            var json = RabbitHomeDefaults.Base64Encoding.DecodeStringFrom(result.Content);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var info = HomeResponseInfo.FromJsonString(json);
                return new QueryResult { Ok = true, SucInfo = null, FailInfo = info };
            }

            var backInfo = QueryRouteBackInfo.FromJsonString(json);
            backInfo.ComputeOpenSk(m_QueryInfo.TempAesKey);
            return new QueryResult { Ok = true, SucInfo = backInfo, FailInfo = null };
        }
    }
}