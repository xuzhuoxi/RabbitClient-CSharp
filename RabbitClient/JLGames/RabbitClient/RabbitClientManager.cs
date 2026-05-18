using System;
using System.Threading;
using System.Threading.Tasks;
using JLGames.Infra.Crypto.Key;
using JLGames.Infra.Crypto.Symmetric;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.RabbitClient.Home;
using JLGames.RabbitClient.Server;

namespace JLGames.RabbitClient
{
    /// <summary>
    /// Orchestrates home route query, socket connection, and encrypted messaging.
    /// 协调主页路由查询、Socket 连接与加密消息通信。
    /// </summary>
    public class RabbitClientManager : EventDispatcher, IDisposable
    {
        private readonly HomeSettings m_HomeSettings;
        private readonly IHttpClientProxy m_HomeHttpProxy;

        private SynchronizationContext m_ThreadSocketContext;
        private RabbitHomeClient m_HomeClient;
        private QueryRouteInfo m_QueryInfo;
        private QueryResult m_QueryResult;
        private RabbitSocketServer m_SocketServer;
        private RabbitSocketClient m_SocketClient;

        /// <summary>
        /// Home server connection and crypto settings.
        /// 主页服务器连接与加密配置。
        /// </summary>
        public HomeSettings HomeSettings => m_HomeSettings;

        /// <summary>
        /// HTTP proxy used for home API requests.
        /// 用于主页 API 请求的 HTTP 代理。
        /// </summary>
        public IHttpClientProxy HomeHttpProxy => m_HomeHttpProxy;

        /// <summary>
        /// Client for querying routes from the home server.
        /// 向主页服务器查询路由的客户端。
        /// </summary>
        public RabbitHomeClient HomeClient => m_HomeClient;

        /// <summary>
        /// Last route query parameters submitted to home.
        /// 最近一次提交给主页的路由查询参数。
        /// </summary>
        public QueryRouteInfo QueryInfo => m_QueryInfo;

        /// <summary>
        /// Low-level socket server connection wrapper.
        /// 底层 Socket 服务器连接封装。
        /// </summary>
        public RabbitSocketServer SocketServer => m_SocketServer;

        /// <summary>
        /// High-level socket client with encryption and event dispatch.
        /// 支持加密与事件分发的高层 Socket 客户端。
        /// </summary>
        public RabbitSocketClient SocketClient => m_SocketClient;

        /// <summary>
        /// Creates a manager with explicit home URL and key options.
        /// 使用显式主页 URL 与密钥选项创建管理器。
        /// </summary>
        /// <param name="homeHttpProxy">HTTP client proxy<br/>HTTP 客户端代理</param>
        /// <param name="homeUrl">Home server base URL<br/>主页服务器地址</param>
        /// <param name="usePost">Whether to use POST for home API<br/>主页 API 是否使用 POST</param>
        /// <param name="enableKey">Whether RSA key encryption is enabled<br/>是否启用 RSA 密钥加密</param>
        /// <param name="isPemKey">Whether the public key is PEM format<br/>公钥是否为 PEM 格式</param>
        /// <param name="pubKeyPath">Path to public key file<br/>公钥文件路径</param>
        /// <param name="pubKeyContent">Inline public key content<br/>内联公钥内容</param>
        public RabbitClientManager(IHttpClientProxy homeHttpProxy, string homeUrl, bool usePost, bool enableKey, bool isPemKey, string pubKeyPath,
            string pubKeyContent)
        {
            m_HomeHttpProxy = homeHttpProxy ?? throw new ArgumentNullException(nameof(homeHttpProxy));
            m_HomeSettings = new HomeSettings(homeUrl, usePost, enableKey, isPemKey);

            m_HomeClient = new RabbitHomeClient(homeHttpProxy, homeUrl, usePost);
            m_HomeSettings.SetPublicKeyPath(pubKeyPath);
            m_HomeSettings.SetPublicKeyContent(pubKeyContent);
        }

        /// <summary>
        /// Creates a manager from an existing home settings object.
        /// 根据已有主页配置对象创建管理器。
        /// </summary>
        /// <param name="homeHttpProxy">HTTP client proxy<br/>HTTP 客户端代理</param>
        /// <param name="homeSettings">Home settings instance<br/>主页配置实例</param>
        public RabbitClientManager(IHttpClientProxy homeHttpProxy, HomeSettings homeSettings)
        {
            m_HomeHttpProxy = homeHttpProxy ?? throw new ArgumentNullException(nameof(homeHttpProxy));
            m_HomeSettings = homeSettings ?? throw new ArgumentNullException(nameof(homeSettings));
            m_HomeClient = new RabbitHomeClient(homeHttpProxy, homeSettings.HomeUrl, homeSettings.UsePost);
        }

        /// <summary>
        /// Releases socket, home client, and dispatcher resources.
        /// 释放 Socket、主页客户端与分发器资源。
        /// </summary>
        public override void Dispose()
        {
            m_SocketClient?.Dispose();
            m_SocketClient = null;
            m_SocketServer?.Dispose();
            m_SocketServer = null;
            m_HomeClient?.Dispose();
            m_HomeClient = null;
            base.Dispose();
        }

        /// <summary>
        /// Sets the synchronization context for socket callbacks.
        /// 设置 Socket 回调的同步上下文。
        /// </summary>
        /// <param name="context">Synchronization context<br/>同步上下文</param>
        public void SetThreadSocketContext(SynchronizationContext context)
        {
            m_ThreadSocketContext = context;
            if (null != m_SocketServer)
            {
                m_SocketServer.SetThreadSocketContext(context);
            }
        }

        /// <summary>
        /// Connects via home route query using platform, type, and a temporary AES key.
        /// 通过主页路由查询连接，使用平台、类型与临时 AES 密钥。
        /// </summary>
        /// <param name="platformId">Platform identifier<br/>平台标识</param>
        /// <param name="typeName">Connection or game type name<br/>连接或游戏类型名</param>
        /// <param name="tempAesKey">Temporary AES session key bytes<br/>临时 AES 会话密钥字节</param>
        /// <returns>Async connect task<br/>异步连接任务</returns>
        public Task ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)
        {
            return ConnectThroughHome(new QueryRouteInfo { PlatformId = platformId, TypeName = typeName, TempAesKey = tempAesKey });
        }

        /// <summary>
        /// Connects via home route query, optionally deriving a random AES key.
        /// 通过主页路由查询连接，可选派生随机 AES 密钥。
        /// </summary>
        /// <param name="platformId">Platform identifier<br/>平台标识</param>
        /// <param name="typeName">Connection or game type name<br/>连接或游戏类型名</param>
        /// <param name="randomAesKey">Whether to derive a random AES key<br/>是否派生随机 AES 密钥</param>
        /// <param name="passphrase">PBKDF2 passphrase when randomAesKey is true<br/>randomAesKey 为 true 时的 PBKDF2 口令</param>
        /// <returns>Async connect task<br/>异步连接任务</returns>
        public Task ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "RabbitClient")
        {
            var queryRouteInfo = new QueryRouteInfo { PlatformId = platformId, TypeName = typeName };
            if (randomAesKey)
            {
                queryRouteInfo.TempAesKey = KeyDerivation.DeriveKeyPbkdf2StrDefault(passphrase);
            }

            return ConnectThroughHome(queryRouteInfo);
        }

        /// <summary>
        /// Connects via home route query with full query parameters.
        /// 使用完整查询参数通过主页路由查询并连接。
        /// </summary>
        /// <param name="queryInfo">Route query parameters<br/>路由查询参数</param>
        /// <returns>Async connect task<br/>异步连接任务</returns>
        public Task ConnectThroughHome(QueryRouteInfo queryInfo)
        {
            m_QueryInfo = queryInfo;
            PrepareConnect();
            return DoQueryFromHome();
        }

        private void PrepareConnect()
        {
            m_SocketClient?.Dispose();
            m_SocketServer?.Dispose();

            m_SocketServer = new RabbitSocketServer();
            m_SocketServer.SetThreadSocketContext(m_ThreadSocketContext);
            m_SocketClient = new RabbitSocketClient(m_SocketServer);
        }

        private async Task DoQueryFromHome()
        {
            try
            {
                QueryResult result;
                if (m_HomeSettings.EnableKey)
                {
                    var publicCipher = RabbitHomeUtils.LoadHomePublicRsa(m_HomeSettings);
                    m_HomeClient.SetPublicRsa(publicCipher);
                    result = await m_HomeClient.QueryFromHome(m_QueryInfo);
                }
                else
                    result = await m_HomeClient.QueryFromHome(m_QueryInfo);

                HandleHomeResponse(result);
            }
            catch (Exception e)
            {
                // 触发错误事件
                DispatchEvent(RabbitClientManagerEvents.EventOnProgressHome
                    , new RabbitClientManagerEvents.ProgressEventData<QueryResult>
                    {
                        Suc = false, Error = e
                    });
                DispatchEvent(RabbitClientManagerEvents.EventOnConnectFinish, false);
            }
        }

        private void HandleHomeResponse(QueryResult result)
        {
            m_QueryResult = result;
            if (!result.Ok)
            {
                if (result.KeyError || result.ParamError || result.TimeOut)
                {
                    DispatchEvent(RabbitClientManagerEvents.EventOnProgressHome
                        , new RabbitClientManagerEvents.ProgressEventData<QueryResult>
                        {
                            Suc = false, Data = result
                        });
                    DispatchEvent(RabbitClientManagerEvents.EventOnConnectFinish, false);
                    return;
                }
            }



            if (null != result.FailInfo)
            {
                DispatchEvent(RabbitClientManagerEvents.EventOnProgressHome
                    , new RabbitClientManagerEvents.ProgressEventData<QueryResult> { Suc = false, Data = result });
                DispatchEvent(RabbitClientManagerEvents.EventOnConnectFinish, false);
                return;
            }

            DispatchEvent(RabbitClientManagerEvents.EventOnProgressHome
                , new RabbitClientManagerEvents.ProgressEventData<QueryResult> { Suc = true, Data = result });
            DoConnectServer(result.SucInfo);
        }

        private void DoConnectServer(QueryRouteBackInfo serverInfo)
        {
            AddServerListeners();
            m_SocketServer.ConnectServer(serverInfo);
        }

        private void AddServerListeners()
        {
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionOpenSuc, OnServerOpenSuc);
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionOpenFail, OnServerOpenFail);
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionClose, OnServerClose);
        }

        private void RemoveServerListeners()
        {
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionClose, OnServerClose);
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionOpenFail, OnServerOpenFail);
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionOpenSuc, OnServerOpenSuc);
        }

        private void OnServerClose(EventData evd)
        {
            RemoveServerListeners();
        }

        private void OnServerOpenFail(EventData evd)
        {
            RemoveServerListeners();
            var info = (SocketEvents.SocketConnEventInfo)evd.Data;
            DispatchEvent(RabbitClientManagerEvents.EventOnProgressServer,
                new RabbitClientManagerEvents.ProgressEventData<SocketEvents.SocketConnEventInfo>
                {
                    Suc = false, Data = info
                });
            DispatchEvent(RabbitClientManagerEvents.EventOnConnectFinish, false);
        }

        private void OnServerOpenSuc(EventData evd)
        {
            RemoveServerListeners();
            var info = (SocketEvents.SocketConnEventInfo)evd.Data;
            if (m_QueryResult.SucInfo?.OpenSk != null)
            {
                var cipher = new AesCipher(m_QueryResult.SucInfo.OpenSk);
                m_SocketClient.SetSymmetricCipher(cipher);
            }

            m_SocketClient.StartReceiving();
            DispatchEvent(RabbitClientManagerEvents.EventOnProgressServer,
                new RabbitClientManagerEvents.ProgressEventData<SocketEvents.SocketConnEventInfo>
                {
                    Suc = true, Data = info
                });
            DispatchEvent(RabbitClientManagerEvents.EventOnConnectFinish, true);
        }
    }
}
