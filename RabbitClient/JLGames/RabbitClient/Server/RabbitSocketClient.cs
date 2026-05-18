using System;
using JLGames.Infra.Crypto;
using JLGames.Infra.Event;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server
{
    /// <summary>
    /// High-level Rabbit socket client with encryption and event dispatch.
    /// 高层 Rabbit Socket 客户端，支持加密与事件分发。
    /// </summary>
    public class RabbitSocketClient : IDisposable
    {
        private ICipher m_SymmetricCipher;
        private RabbitSocketServer m_SocketServer;
        private IEventDispatcher m_Dispatcher;
        private EventDispatcherPool m_DispatcherPool;

        /// <summary>
        /// Whether the underlying socket is connected.
        /// 底层 Socket 是否已连接。
        /// </summary>
        public bool Connected => m_SocketServer.Connected;

        /// <summary>
        /// Main event dispatcher for client lifecycle events.
        /// 客户端生命周期事件的主事件分发器。
        /// </summary>
        public IEventDispatcher EventDispatcher => m_Dispatcher;

        /// <summary>
        /// Creates a client bound to the given socket server.
        /// 创建绑定到指定 Socket 服务器的客户端。
        /// </summary>
        /// <param name="socketServer">Socket server instance<br/>Socket 服务器实例</param>
        public RabbitSocketClient(RabbitSocketServer socketServer)
        {
            m_SocketServer = socketServer ?? throw new ArgumentNullException(nameof(socketServer));
            m_Dispatcher = new EventDispatcher();
            m_DispatcherPool = new EventDispatcherPool();
        }

        /// <summary>
        /// Gets or creates an extension-scoped event dispatcher.
        /// 获取或创建按扩展名隔离的事件分发器。
        /// </summary>
        /// <param name="extensionName">Extension name<br/>扩展名</param>
        /// <returns>Extension event dispatcher<br/>扩展事件分发器</returns>
        public IEventDispatcher GetExtensionDispatcher(string extensionName)
        {
            return m_DispatcherPool.GetInstance(extensionName, true);
        }

        /// <summary>
        /// Sets the symmetric cipher used to encrypt and decrypt messages.
        /// 设置用于加解密消息的对称加密器。
        /// </summary>
        /// <param name="cipher">Symmetric cipher implementation<br/>对称加密实现</param>
        public void SetSymmetricCipher(ICipher cipher)
        {
            m_SymmetricCipher = cipher;
        }

        /// <summary>
        /// Starts listening for incoming server messages.
        /// 开始监听服务端消息。
        /// </summary>
        public void StartReceiving()
        {
            if (null != m_SocketServer)
            {
                m_SocketServer.AddEventListener(RabbitSocketServerEvents.EventOnServerMessage, OnReceivedMessage);
            }
        }

        /// <summary>
        /// Stops listening for incoming server messages.
        /// 停止监听服务端消息。
        /// </summary>
        public void StopReceiving()
        {
            if (null != m_SocketServer)
            {
                m_SocketServer.RemoveEventListener(RabbitSocketServerEvents.EventOnServerMessage, OnReceivedMessage);
            }
        }

        /// <summary>
        /// Encodes and sends a Rabbit message to the server.
        /// 编码并向服务器发送 Rabbit 消息。
        /// </summary>
        /// <param name="msg">Message writer with payload<br/>包含载荷的消息写入器</param>
        public void SendMessage(IRabbitMessageWriter msg)
        {
            if (null == m_SocketServer || !m_SocketServer.Connected || null == msg) return;
            var msgBytes = msg.ToMessageBytes();
            if (null == msgBytes || msgBytes.Length == 0) return;
            var msgContent = new RabbitSocketClientEvents.MessageContent();
            msgContent.SetHeaderInfo(msg.Extension, msg.ProtoId, msg.ClientId);
            msgContent.SetMessageContent(msgBytes);
            m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientSendMessagePrepare, msgContent);
            try
            {
                if (null != m_SymmetricCipher)
                {
                    msgBytes = m_SymmetricCipher.Encrypt(msgBytes);
                    msgContent.SetMessageContent(msgBytes);
                }

                m_SocketServer.SocketServer.SendMessage(msgBytes);
                m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientSendMessage, msgContent);
            }
            catch (Exception e)
            {
                m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientReceiveMessageFailed,
                    new RabbitSocketClientEvents.FailedInfo { IsSend = true, FailedException = e, OriginalBytes = msgBytes });
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
                GetExtensionDispatcher(msgReader.Extension).DispatchEvent(msgReader.ProtoId, msgReader);
                m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientReceiveMessage, msgReader);
            }
            catch (Exception e)
            {
                m_Dispatcher.DispatchEvent(RabbitSocketClientEvents.EventOnClientReceiveMessageFailed,
                    new RabbitSocketClientEvents.FailedInfo { IsSend = false, FailedException = e, OriginalBytes = msgBytes });
            }
        }

        /// <summary>
        /// Releases listeners and dispatcher resources.
        /// 释放监听器与分发器资源。
        /// </summary>
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
