using System;
using System.Net.Sockets;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.Infra.Threadx;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClient.Server
{
    public class RabbitSocketServer : EventDispatcher
    {
        private QueryRouteBackInfo m_ServerInfo;
        private FixedThreadContext m_ThreadContext;

        private ISocketClient m_SocketServer;
        private bool m_Connecting;
        private bool m_Connected;

        public ISocketClient SocketServer => m_SocketServer;

        /// <summary>
        /// 正在连接 或 正在断开连接
        /// </summary>
        public bool Connecting => m_Connecting;

        /// <summary>
        /// 连接中
        /// </summary>
        public bool Connected => m_Connected;

        public RabbitSocketServer()
        {
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
                m_SocketServer.SetContext(context);
            }
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverInfo"></param>
        public void ConnectServer(QueryRouteBackInfo serverInfo)
        {
            if (m_Connecting || m_Connected) return;
            if (null == serverInfo) throw new ArgumentNullException(nameof(serverInfo));
            if (null != m_ServerInfo) return;
            m_ServerInfo = serverInfo;
            StartConnect();
        }

        public override void Dispose()
        {
            DisconnectServer();
            base.Dispose();
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        public void DisconnectServer()
        {
            if (!m_Connected) return;
            m_Connecting = true;
            try
            {
                if (null != m_SocketServer)
                {
                    m_SocketServer.RemoveEventListener(SocketEvents.EventOnConnectionClose, OnConnectionClose);
                    m_SocketServer.RemoveEventListener(SocketEvents.EventOnMessageReceivedEnd, OnReceivedEnd);
                    m_SocketServer.RemoveEventListener(SocketEvents.EventOnMessageReceived, OnReceivedMessage);

                    m_SocketServer.RemoveEventListener(SocketEvents.EventOnConnectionOpen, OnConnect);
                    m_SocketServer.DisconnectServer();
                }
            }
            finally
            {
                m_Connecting = false;
                m_Connected = false;
                m_ServerInfo = null;
                m_SocketServer = null;
            }
        }

        private void StartConnect()
        {
            m_Connecting = true;
            m_SocketServer =
                SocketFactory.CreateSocketClient(m_ServerInfo.Id, RabbitServerDefaults.LittleEndian, RabbitServerDefaults.ApmMode);
            m_SocketServer.SetContext(m_ThreadContext);

            m_SocketServer.OnceEventListener(SocketEvents.EventOnConnectionOpen, OnConnect);
            m_SocketServer.ConnectServer(new SocketParams
            {
                Network = SocketNetworks.GetNetwork(m_ServerInfo.OpenNetwork),
                RemoteAddress = m_ServerInfo.OpenAddr,
            });
        }

        private void OnConnect(EventData evd)
        {
            var info = (SocketEvents.SocketConnEventInfo)evd.Data;
            if (!info.Suc || info.Error != SocketError.Success || info.Exception != null)
            {
                OnConnectFail(info);
                return;
            }

            OnConnectSuc(info);
        }

        private void OnConnectFail(SocketEvents.SocketConnEventInfo info)
        {
            m_ServerInfo = null;
            m_Connecting = false;
            m_Connected = false;
            DispatchEvent(RabbitSocketServerEvents.EventOnConnectionOpenFail, info);
        }

        private void OnConnectSuc(SocketEvents.SocketConnEventInfo info)
        {
            m_Connecting = false;
            m_Connected = true;
            DispatchEvent(RabbitSocketServerEvents.EventOnConnectionOpenSuc, info);
            m_SocketServer.AddEventListener(SocketEvents.EventOnMessageReceived, OnReceivedMessage);
            m_SocketServer.AddEventListener(SocketEvents.EventOnMessageReceivedEnd, OnReceivedEnd);
            m_SocketServer.AddEventListener(SocketEvents.EventOnConnectionClose, OnConnectionClose);
            m_SocketServer.StartReceiving();
        }

        private void OnReceivedMessage(EventData evd)
        {
            var message = (byte[])evd.Data;
            DispatchEvent(RabbitSocketServerEvents.EventOnServerMessage, message);
        }

        private void OnReceivedEnd(EventData evd)
        {
            m_SocketServer.StopReceiving();
            m_SocketServer.RemoveEventListener(SocketEvents.EventOnMessageReceivedEnd, OnReceivedEnd);
            m_SocketServer.RemoveEventListener(SocketEvents.EventOnMessageReceived, OnReceivedMessage);
        }

        private void OnConnectionClose(EventData evd)
        {
            m_SocketServer.RemoveEventListener(SocketEvents.EventOnConnectionClose, OnConnectionClose);
            m_Connected = false;
            m_Connecting = false;
            m_ServerInfo = null;
            DispatchEvent(RabbitSocketServerEvents.EventOnConnectionClose, evd.Data);
        }
    }
}
