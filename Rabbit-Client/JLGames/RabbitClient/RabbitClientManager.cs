using System.Threading.Tasks;
using JLGames.Infra.Crypto.Key;
using JLGames.Infra.Crypto.Symmetric;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.RabbitClient.Home;
using JLGames.RabbitClient.Server;

namespace JLGames.RabbitClient
{
    public class RabbitClientManager : EventDispatcher
    {
        private readonly HomeSettings m_HomeSettings;
        private QueryRouteInfo m_QueryInfo;
        private QueryResult m_QueryResult;
        private RabbitHomeClient m_HomeClient;
        private RabbitSocketServer m_SocketServer;
        private RabbitSocketClient m_SocketClient;

        public HomeSettings HomeSettings => m_HomeSettings;
        public QueryRouteInfo QueryInfo => m_QueryInfo;
        public RabbitHomeClient HomeClient => m_HomeClient;
        public RabbitSocketServer SocketServer => m_SocketServer;
        public RabbitSocketClient SocketClient => m_SocketClient;

        public RabbitClientManager(string homeUrl, bool usePost, bool enableKey, bool isPemKey, string publicKeyPath)
        {
            m_HomeSettings = new HomeSettings(homeUrl, usePost, enableKey, isPemKey, publicKeyPath);
        }

        public RabbitClientManager(HomeSettings homeSettings)
        {
            m_HomeSettings = homeSettings;
        }

        public Task ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)
        {
            return ConnectThroughHome(new QueryRouteInfo { PlatformId = platformId, TypeName = typeName, TempAesKey = tempAesKey });
        }

        public Task ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "Rabbit-Client")
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
            m_HomeClient = new RabbitHomeClient(m_HomeSettings.HomeUrl, m_HomeSettings.UsePost);
            m_SocketServer = new RabbitSocketServer();
            m_SocketClient = new RabbitSocketClient(m_SocketServer);
        }

        private async Task DoQueryFromHome()
        {
            QueryResult result;
            if (m_HomeSettings.EnableKey)
                result = await m_HomeClient.QueryFromHome(m_QueryInfo, m_HomeSettings.IsPemKey, m_HomeSettings.PublicKeyPath);
            else
                result = await m_HomeClient.QueryFromHome(m_QueryInfo);

            HandleHomeResponse(result);
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
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionOpenSuc, OnServerOpenSuc);
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionOpenFail, OnServerOpenFail);
            m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnConnectionClose, OnServerClose);
            m_SocketServer.ConnectServer(serverInfo);
        }

        private void OnServerClose(EventData evd)
        {
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionClose, OnServerClose);
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionOpenFail, OnServerOpenFail);
            m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnConnectionOpenSuc, OnServerOpenSuc);
        }

        private void OnServerOpenFail(EventData evd)
        {
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
            var info = (SocketEvents.SocketConnEventInfo)evd.Data;
            if (null != m_QueryResult.SucInfo.OpenSk)
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
