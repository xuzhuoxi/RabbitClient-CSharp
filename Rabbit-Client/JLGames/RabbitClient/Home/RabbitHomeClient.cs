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
    public class RabbitHomeClient : IDisposable
    {
        private readonly IHttpClientProxy m_HttpProxy;
        private readonly string m_HomeUrl;
        private readonly Uri m_HomeUri;

        private readonly bool m_UsePost;
        private readonly TimeSpan m_Timeout;

        private QueryRouteInfo m_QueryInfo;
        private IRsaPublicCipher m_PubRsaCipher;

        public string HomeUrl => m_HomeUrl;

        public RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost)
        {
            m_HttpProxy = httpProxyProxy;
            m_HomeUrl = homeUrl.Trim();
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = TimeSpan.FromSeconds(100);
        }

        public RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost, TimeSpan timeout)
        {
            m_HttpProxy = httpProxyProxy;
            m_HomeUrl = homeUrl.Trim();
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = timeout;
        }

        public void Dispose()
        {
            m_PubRsaCipher?.Dispose();
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
        /// <param name="publicKeyContent"></param>
        /// <returns></returns>
        public Task<QueryResult> QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath, string publicKeyContent)
        {
            var pubCipher = RabbitHomeUtils.LoadHomePublicRsa(isPemKey, publicKeyPath, publicKeyContent);
            if (null == pubCipher)
            {
                return Task.FromResult(new QueryResult { Ok = false, KeyError = true });
            }

            return QueryFromHome(queryInfo, pubCipher);
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
            var result = await m_HttpProxy.GetBytesAsync(m_HomeUri, pattern, m_Timeout);
            return HandleHomeResponse(result);
        }

        private async Task<QueryResult> DoPost(byte[] bytes)
        {
            var base64 = RabbitHomeDefaults.Base64Encoding.EncodeToString(bytes);
            var pattern = $"{RabbitHomeDefaults.HttpPatternRoute}";
            var value = new Dictionary<string, string>
            {
                { RabbitHomeDefaults.HttpKeyQuery, base64 }
            };
            var result = await m_HttpProxy.PostBytesAsync(m_HomeUri, pattern, value, m_Timeout);
            return HandleHomeResponse(result);
        }

        private QueryResult HandleHomeResponse(HttpResult<byte[]> result)
        {
            if (result.Timeout)
                return new QueryResult { Ok = false, TimeOut = true };

            if (result.StatusCode != HttpStatusCode.OK)
            {
                if (null == result.Content)
                {
                    return new QueryResult
                    {
                        Ok = false, SucInfo = null,
                        FailInfo = new HomeResponseInfo(-1, "No response content.", "")
                    };
                }

                var failJson = RabbitHomeDefaults.Base64Encoding.DecodeStringFrom(result.Content);
                var info = HomeResponseInfo.FromJsonString(failJson);
                return new QueryResult { Ok = false, SucInfo = null, FailInfo = info };
            }

            var sucJson = RabbitHomeDefaults.Base64Encoding.DecodeStringFrom(result.Content);
            var backInfo = QueryRouteBackInfo.FromJsonString(sucJson);
            backInfo.ComputeOpenSk(m_QueryInfo.TempAesKey);
            return new QueryResult { Ok = true, SucInfo = backInfo, FailInfo = null };
        }
    }
}
