using System;
using System.Threading.Tasks;
using JLGames.Infra.Crypto.Asymmetric;
using JLGames.Infra.Crypto.Key;
using JLGames.Infra.Crypto.Symmetric;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.Infra.Threadx;
using JLGames.RabbitClient.Home;
using JLGames.RabbitClient.Server;

namespace JLGames.RabbitClient
{
    public class RabbitClientManager : EventDispatcher, IDisposable
    {
        private readonly HomeSettings m_HomeSettings;
        private readonly IHttpClientProxy m_HomeHttpProxy;

        private FixedThreadContext m_ThreadContext;
        private RabbitHomeClient m_HomeClient;
        private QueryRouteInfo m_QueryInfo;
        private QueryResult m_QueryResult;
        private RabbitSocketServer m_SocketServer;
        private RabbitSocketClient m_SocketClient;

        public HomeSettings HomeSettings => m_HomeSettings;
        public IHttpClientProxy HomeHttpProxy => m_HomeHttpProxy;
        public RabbitHomeClient HomeClient => m_HomeClient;
        public QueryRouteInfo QueryInfo => m_QueryInfo;
        public RabbitSocketServer SocketServer => m_SocketServer;
        public RabbitSocketClient SocketClient => m_SocketClient;

        public RabbitClientManager(IHttpClientProxy homeHttpProxy, string homeUrl, bool usePost, bool enableKey, bool isPemKey, string pubKeyPath,
            string pubKeyContent)
        {
            m_HomeHttpProxy = homeHttpProxy ?? throw new ArgumentNullException(nameof(homeHttpProxy));
            m_HomeSettings = new HomeSettings(homeUrl, usePost, enableKey, isPemKey);

            m_HomeClient = new RabbitHomeClient(homeHttpProxy, homeUrl, usePost);
            m_HomeSettings.SetPublicKeyPath(pubKeyPath);
            m_HomeSettings.SetPublicKeyContent(pubKeyContent);
        }

        public RabbitClientManager(IHttpClientProxy homeHttpProxy, HomeSettings homeSettings)
        {
            m_HomeHttpProxy = homeHttpProxy ?? throw new ArgumentNullException(nameof(homeHttpProxy));
            m_HomeSettings = homeSettings ?? throw new ArgumentNullException(nameof(homeSettings));
            m_HomeClient = new RabbitHomeClient(homeHttpProxy, homeSettings.HomeUrl, homeSettings.UsePost);
        }

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
        /// 设置线程上下文
        /// </summary>
        /// <param name="context"></param>
        public void SetThreadContext(FixedThreadContext context)
        {
            m_ThreadContext = context;
            if (null != m_SocketServer)
            {
                m_SocketServer.SetThreadContext(context);
            }
        }

        public Task ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)
        {
            return ConnectThroughHome(new QueryRouteInfo { PlatformId = platformId, TypeName = typeName, TempAesKey = tempAesKey });
        }

        public Task ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "RabbitClient")
        {
            var queryRouteInfo = new QueryRouteInfo { PlatformId = platformId, TypeName = typeName };
            if (randomAesKey)
            {
                queryRouteInfo.TempAesKey = KeyDerivation.DeriveKeyPbkdf2StrDefault(passphrase);
            }

            return ConnectThroughHome(queryRouteInfo);
        }

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
            m_SocketServer.SetThreadContext(m_ThreadContext);
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
