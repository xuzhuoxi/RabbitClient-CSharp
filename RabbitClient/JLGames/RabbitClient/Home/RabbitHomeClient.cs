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
    /// <summary>
    /// Rabbit-Home client for querying available Rabbit-Server instance routes.
    /// Rabbit-Home 客户端，用于向 Home 服务查询可用的 Rabbit-Server 实例路由
    /// </summary>
    public class RabbitHomeClient : IDisposable
    {
        private readonly IHttpClientProxy m_HttpProxy;
        private readonly string m_HomeUrl;
        private readonly Uri m_HomeUri;

        private readonly bool m_UsePost;
        private readonly TimeSpan m_Timeout;

        private QueryRouteInfo m_QueryInfo;
        private IRsaPublicCipher m_PubRsaCipher;

        /// <summary>
        /// Rabbit-Home service URL.
        /// Rabbit-Home 服务地址
        /// </summary>
        public string HomeUrl => m_HomeUrl;

        /// <summary>
        /// Creates a Rabbit-Home client (default timeout 100 seconds).
        /// 创建 Rabbit-Home 客户端（默认超时 100 秒）
        /// </summary>
        /// <param name="homeUrl">Rabbit-Home service URL<br/> Rabbit-Home 服务地址</param>
        /// <param name="usePost">Use POST when true, otherwise GET<br/> 为 true 时使用 POST，否则使用 GET</param>
        public RabbitHomeClient(string homeUrl, bool usePost)
        {
            m_HomeUrl = homeUrl.Trim();
            m_HttpProxy = new HttpClientProxy(m_HomeUrl);
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = TimeSpan.FromSeconds(100);
        }

        /// <summary>
        /// Creates a Rabbit-Home client.
        /// 创建 Rabbit-Home 客户端
        /// </summary>
        /// <param name="homeUrl">Rabbit-Home service URL<br/> Rabbit-Home 服务地址</param>
        /// <param name="usePost">Use POST when true, otherwise GET<br/> 为 true 时使用 POST，否则使用 GET</param>
        /// <param name="timeout">HTTP request timeout<br/> HTTP 请求超时时间</param>
        public RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)
        {
            m_HomeUrl = homeUrl.Trim();
            m_HttpProxy = new HttpClientProxy(m_HomeUrl);
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = timeout;
        }

        /// <summary>
        /// Creates a Rabbit-Home client with a custom HTTP proxy (default timeout 100 seconds).
        /// 使用自定义 HTTP 代理创建 Rabbit-Home 客户端（默认超时 100 秒）
        /// </summary>
        /// <param name="httpProxyProxy">HTTP client proxy<br/> HTTP 客户端代理</param>
        /// <param name="homeUrl">Rabbit-Home service URL<br/> Rabbit-Home 服务地址</param>
        /// <param name="usePost">Use POST when true, otherwise GET<br/> 为 true 时使用 POST，否则使用 GET</param>
        public RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost)
        {
            m_HttpProxy = httpProxyProxy;
            m_HomeUrl = homeUrl.Trim();
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = TimeSpan.FromSeconds(100);
        }

        /// <summary>
        /// Creates a Rabbit-Home client with a custom HTTP proxy.
        /// 使用自定义 HTTP 代理创建 Rabbit-Home 客户端
        /// </summary>
        /// <param name="httpProxyProxy">HTTP client proxy<br/> HTTP 客户端代理</param>
        /// <param name="homeUrl">Rabbit-Home service URL<br/> Rabbit-Home 服务地址</param>
        /// <param name="usePost">Use POST when true, otherwise GET<br/> 为 true 时使用 POST，否则使用 GET</param>
        /// <param name="timeout">HTTP request timeout<br/> HTTP 请求超时时间</param>
        public RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost, TimeSpan timeout)
        {
            m_HttpProxy = httpProxyProxy;
            m_HomeUrl = homeUrl.Trim();
            m_HomeUri = new Uri(m_HomeUrl);
            m_UsePost = usePost;
            m_Timeout = timeout;
        }

        /// <summary>
        /// Releases RSA public cipher and other resources.
        /// 释放 RSA 公钥加密器等资源
        /// </summary>
        public void Dispose()
        {
            m_PubRsaCipher?.Dispose();
        }

        /// <summary>
        /// Sets the RSA public key cipher.
        /// 设置 RSA 公钥加密器
        /// </summary>
        /// <param name="pub">RSA public key cipher instance<br/> RSA 公钥加密器实例</param>
        public void SetPublicRsa(IRsaPublicCipher pub)
        {
            m_PubRsaCipher = pub;
        }

        /// <summary>
        /// Sets the RSA public key.
        /// 设置 RSA 公钥
        /// </summary>
        /// <param name="pubKey">.NET RSA public key<br/> .NET RSA 公钥对象</param>
        public void SetPublicRsa(RSA pubKey)
        {
            SetPublicRsa(new RsaPublicCipher(pubKey));
        }

        /// <summary>
        /// Queries Rabbit-Home for an available instance (loads public key from path or content).
        /// 向 Rabbit-Home 服务器查询可用实例（从路径或内容加载公钥）
        /// </summary>
        /// <param name="queryInfo">Route query parameters<br/> 路由查询参数</param>
        /// <param name="isPemKey">Public key in PEM format when true<br/> 公钥是否为 PEM 格式</param>
        /// <param name="publicKeyPath">Public key file path; mutually exclusive with publicKeyContent<br/> 公钥文件路径，与 publicKeyContent 二选一</param>
        /// <param name="publicKeyContent">Public key text; mutually exclusive with publicKeyPath<br/> 公钥文本内容，与 publicKeyPath 二选一</param>
        /// <returns>Query result; KeyError is true if public key load fails<br/> 查询结果，公钥加载失败时 KeyError 为 true</returns>
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
        /// Queries Rabbit-Home for an available instance.
        /// 向 Rabbit-Home 服务器查询可用实例
        /// </summary>
        /// <param name="queryInfo">Route query parameters<br/> 路由查询参数</param>
        /// <param name="publicCipher">RSA public cipher for encrypting the request body<br/> 用于加密请求体的 RSA 公钥加密器</param>
        /// <returns>Query result<br/> 查询结果</returns>
        public Task<QueryResult> QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)
        {
            SetPublicRsa(publicCipher);
            return QueryFromHome(queryInfo);
        }

        /// <summary>
        /// Queries Rabbit-Home for an available instance (uses configured public key; no encryption if unset).
        /// 向 Rabbit-Home 服务器查询可用实例（使用已设置的公钥，未设置则不加密请求体）
        /// </summary>
        /// <param name="queryInfo">Route query parameters<br/> 路由查询参数</param>
        /// <returns>Query result; ParamError is true when queryInfo is null<br/> 查询结果，参数为 null 时 ParamError 为 true</returns>
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
