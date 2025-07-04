using JLGames.Infra.Event;
using JLGames.Infra.Threadx;

namespace JLGames.RabbitClient.Server
{
    public class RabbitSocketClient : EventDispatcher
    {
        private readonly RabbitSocketServer m_SocketServer;
        private FixedThreadContext m_Context;

        public bool Connected => m_SocketServer.Connected;

        public RabbitSocketClient(RabbitSocketServer socketServer)
        {
            m_SocketServer = socketServer;
        }

        /// <summary>
        /// 设置线程上下文
        /// </summary>
        /// <param name="context"></param>
        public void SetThreadContext(FixedThreadContext context)
        {
            m_Context = context;
            if (null != m_SocketServer)
            {
                m_SocketServer.SetThreadContext(context);
            }
        }

        /// <summary>
        /// 开始接收消息
        /// </summary>
        public void StartReceiving()
        {
            if (null != m_SocketServer)
            {
                m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnServerMessage, OnReceivedMessage);
            }
        }

        /// <summary>
        /// 停止接收消息
        /// </summary>
        public void StopReceiving()
        {
            if (null != m_SocketServer)
            {
                m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnServerMessage, OnReceivedMessage);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(IRabbitMessageWriter msg)
        {
            if (!m_SocketServer.Connected) return;
            m_SocketServer.SocketServer.SendMessage(msg.ToMessageBytes());
        }

        private void OnReceivedMessage(EventData evd)
        {
            var msg = evd.Data as byte[];
            var msgReader = new RabbitResponseMsg(RabbitServerDefaults.LittleEndian);
            msgReader.SetMessageBytes(msg);
            msgReader.StartReadData();
            DispatchEvent(RabbitSocketClientEvents.EventOnClientMessage, msg);
        }
    }
}