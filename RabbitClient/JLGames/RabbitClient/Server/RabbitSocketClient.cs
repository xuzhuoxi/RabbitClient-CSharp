using System;
using JLGames.Infra.Crypto;
using JLGames.Infra.Event;
using JLGames.Infra.Threadx;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server
{
    public class RabbitSocketClient : IDisposable
    {
        private ICipher m_SymmetricCipher;
        private RabbitSocketServer m_SocketServer;
        private IEventDispatcher m_Dispatcher;
        private EventDispatcherPool m_DispatcherPool;
        private FixedThreadContext m_Context;

        public bool Connected => m_SocketServer.Connected;

        public IEventDispatcher EventDispatcher => m_Dispatcher;

        public RabbitSocketClient(RabbitSocketServer socketServer)
        {
            m_SocketServer = socketServer ?? throw new ArgumentNullException(nameof(socketServer));
            m_Dispatcher = new EventDispatcher();
            m_DispatcherPool = new EventDispatcherPool();
        }

        public IEventDispatcher GetExtensionDispatcher(string extensionName)
        {
            return m_DispatcherPool.GetInstance(extensionName, true);
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
        /// 设置对称密钥
        /// </summary>
        /// <param name="cipher"></param>
        public void SetSymmetricCipher(ICipher cipher)
        {
            m_SymmetricCipher = cipher;
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
            if (null == m_SocketServer || !m_SocketServer.Connected || null == msg) return;
            var msgBytes = msg.ToMessageBytes();
            if (null == msgBytes || msgBytes.Length == 0) return;
            // Console.WriteLine($"SendMessage[{msgBytes.Length}]: [{string.Join(" ", msgBytes)}]");
            try
            {
                if (null != m_SymmetricCipher)
                {
                    msgBytes = m_SymmetricCipher.Encrypt(msgBytes);
                    // Console.WriteLine($"SendMessage2[{msgBytes.Length}]: [{string.Join(" ", msgBytes)}]");
                }

                m_SocketServer.SocketServer.SendMessage(msgBytes);
            }
            catch
            {
                // ignored
            }
        }

        private void OnReceivedMessage(EventData evd)
        {
            var msgBytes = evd.Data as byte[];
            try
            {
                if (null != msgBytes && msgBytes.Length > 0 && null != m_SymmetricCipher)
                {
                    msgBytes = m_SymmetricCipher.Decrypt(msgBytes);
                }

                var msgReader = new RabbitResponseMsg(RabbitServerDefaults.LittleEndian);
                msgReader.SetMessageBytes(msgBytes);
                msgReader.StartReadData();
                GetExtensionDispatcher(msgReader.Extension).DispatchEvent(msgReader.ProtoUid, msgReader);
                m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientMessage, msgReader);
            }
            catch
            {
                // ignored
            }
        }

        public void Dispose()
        {
            StopReceiving();

            m_DispatcherPool?.ClearAll();
            m_Dispatcher?.Dispose();

            m_SocketServer = null;
            m_DispatcherPool = null;
            m_Dispatcher = null;
        }
    }
}
