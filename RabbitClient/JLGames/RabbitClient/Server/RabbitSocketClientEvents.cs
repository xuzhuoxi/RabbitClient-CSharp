using System;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server
{
    /// <summary>
    /// Event names and payload types for Rabbit socket client.
    /// Rabbit Socket 客户端的事件名与载荷类型。
    /// </summary>
    public static class RabbitSocketClientEvents
    {
        /// <summary>
        /// Preparing to send message
        /// 准备发送消息
        /// Event data(事件数据)：MessageContent
        /// </summary>
        public const string EventOnClientSendMessagePrepare = "RabbitSocketClientEvents.EventOnClientSendMessagePrepare";

        /// <summary>
        /// Send Message
        /// 发送消息
        /// Event data(事件数据)：MessageContent
        /// </summary>
        public const string EventOnClientSendMessage = "RabbitSocketClientEvents.EventOnClientSendMessage";

        /// <summary>
        /// Data reception processing completed
        /// 数据接收处理结束
        /// Event data(事件数据)：IRabbitResponseMsg
        /// </summary>
        public const string EventOnClientReceiveMessage = "RabbitSocketClientEvents.EventOnClientReceiveMessage";

        /// <summary>
        /// Send or Receive Data Failed event
        /// 收发消息失败事件
        /// Event data(事件数据): FailedInfo
        /// </summary>
        public const string EventOnClientReceiveMessageFailed = "RabbitSocketClientEvents.EventOnClientMessageFailed";

        /// <summary>
        /// Message content passed with client send/receive events.
        /// 客户端收发事件携带的消息内容。
        /// </summary>
        public class MessageContent : IRabbitMessageContent
        {
            private RabbitMessageHeader m_MessageHeader;
            private byte[] m_Content;

            /// <summary>
            /// Extension name from the message header.
            /// 消息头中的扩展名。
            /// </summary>
            public string Extension => m_MessageHeader.Extension;

            /// <summary>
            /// Protocol id from the message header.
            /// 消息头中的协议Id。
            /// </summary>
            public string ProtoId => m_MessageHeader.ProtoId;

            /// <summary>
            /// Client id from the message header.
            /// 消息头中的客户端标识。
            /// </summary>
            public string ClientId => m_MessageHeader.ClientId;

            /// <summary>
            /// Unique protocol id from the message header.
            /// 消息头中的协议唯一Id。
            /// </summary>
            public string ProtoUid => m_MessageHeader.ProtoUid;

            /// <summary>
            /// Raw message payload bytes.
            /// 原始消息载荷字节。
            /// </summary>
            public byte[] Content => m_Content;

            /// <summary>
            /// Comma-separated string representation of payload bytes.
            /// 载荷字节的逗号分隔字符串表示。
            /// </summary>
            public string ContentString
            {
                get
                {
                    if (m_Content == null) return "";
                    return $"[{string.Join(",", m_Content)}]";
                }
            }

            /// <summary>
            /// Sets header fields on the internal message header.
            /// 设置内部消息头的字段。
            /// </summary>
            /// <param name="extension">Extension name<br/>扩展名</param>
            /// <param name="protoId">Protocol id<br/>协议Id</param>
            /// <param name="cid">Client id<br/>客户端标识</param>
            public void SetHeaderInfo(string extension, string protoId, string cid)
            {
                m_MessageHeader.SetHeaderInfo(extension, protoId, cid);
            }

            /// <summary>
            /// Sets the raw message payload bytes.
            /// 设置原始消息载荷字节。
            /// </summary>
            /// <param name="content">Payload bytes<br/>载荷字节</param>
            public void SetMessageContent(byte[] content)
            {
                m_Content = content;
            }
        }

        /// <summary>
        /// Details when a client send or receive operation fails.
        /// 客户端收发失败时的详情。
        /// </summary>
        public struct FailedInfo
        {
            /// <summary>
            /// Is this a sending action?
            /// 是否为发送行为
            /// </summary>
            public bool IsSend;

            /// <summary>
            /// Failed code
            /// 失败代码
            /// </summary>
            public int FailedCode;

            /// <summary>
            /// Original data
            /// 原始数据
            /// </summary>
            public byte[] OriginalBytes;

            /// <summary>
            /// Failed Exception
            /// 失败异常
            /// </summary>
            public Exception FailedException;

            /// <summary>
            /// Returns a diagnostic string for logging.
            /// 返回用于日志的诊断字符串。
            /// </summary>
            /// <returns>Formatted failure info<br/>格式化后的失败信息</returns>
            public override string ToString()
            {
                var strOb = "[]";
                if (null != OriginalBytes)
                    strOb = $"[{string.Join(" ", OriginalBytes)}]";

                return
                    $"{{Send={IsSend}, FailedCode={FailedCode}, OriginalBytes={strOb}, \nFailedException={FailedException}}}";
            }
        }
    }
}
