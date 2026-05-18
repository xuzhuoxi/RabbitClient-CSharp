using System;
using System.Net.Sockets;
using System.Threading;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClient.Server
{
    /// <summary>
    /// Low-level Rabbit socket server connection wrapper with event dispatch.
    /// 底层 Rabbit Socket 服务器连接封装，带事件分发。
    /// </summary>
    public class RabbitSocketServer : EventDispatcher
    {
        private QueryRouteBackInfo m_ServerInfo;
        private SynchronizationContext m_ThreadSocketContext;

        private ISocketClient m_SocketServer;
        private bool m_Connecting;
        private bool m_Connected;

        /// <summary>
        /// Underlying socket client used for I/O.
        /// 用于 I/O 的底层 Socket 客户端。
        /// </summary>
        public ISocketClient SocketServer => m_SocketServer;

        /// <summary>
        /// Whether a connect or disconnect operation is in progress.
        /// 是否正在进行连接或断开操作。
        /// </summary>
        public bool Connecting => m_Connecting;

        /// <summary>
        /// Whether the socket is currently connected.
        /// Socket 当前是否已连接。
        /// </summary>
        public bool Connected => m_Connected;

        /// <summary>
        /// Creates a new socket server instance.
        /// 创建 Socket 服务器实例。
        /// </summary>
        public RabbitSocketServer()
        {
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
                m_SocketServer.SetContext(context);
            }
        }

        /// <summary>
        /// Connects to the game server using route information.
        /// 使用路由信息连接到游戏服务器。
        /// </summary>
        /// <param name="serverInfo">Server route and address info<br/>服务器路由与地址信息</param>
        public void ConnectServer(QueryRouteBackInfo serverInfo)
        {
            if (m_Connecting || m_Connected) return;
            if (null == serverInfo) throw new ArgumentNullException(nameof(serverInfo));
            if (null != m_ServerInfo) return;
            m_ServerInfo = serverInfo;
            StartConnect();
        }

        /// <summary>
        /// Disconnects from the server and releases socket resources.
        /// 断开与服务器的连接并释放 Socket 资源。
        /// </summary>
        public override void Dispose()
        {
            DisconnectServer();
            base.Dispose();
        }

        /// <summary>
        /// Disconnects from the server if currently connected.
        /// 若已连接则断开与服务器的连接。
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
                m_ThreadSocketContext = null;
            }
        }

        private void StartConnect()
        {
            m_Connecting = true;
            m_SocketServer =
                SocketFactory.CreateSocketClient(m_ServerInfo.Id, RabbitServerDefaults.LittleEndian, RabbitServerDefaults.ApmMode);
            m_SocketServer.SetContext(m_ThreadSocketContext);

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
