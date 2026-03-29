using System;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server
{
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

        public class MessageContent : IRabbitMessageContent
        {
            private RabbitMessageHeader m_MessageHeader;
            private byte[] m_Content;
            public string Extension => m_MessageHeader.Extension;
            public string ProtoId => m_MessageHeader.ProtoId;
            public string ClientId => m_MessageHeader.ClientId;
            public string ProtoUid => m_MessageHeader.ProtoUid;
            public byte[] Content => m_Content;

            public string ContentString
            {
                get
                {
                    if (m_Content == null) return "";
                    return $"[{string.Join(",", m_Content)}]";
                }
            }

            public void SetHeaderInfo(string extension, string protoId, string cid)
            {
                m_MessageHeader.SetHeaderInfo(extension, protoId, cid);
            }

            public void SetMessageContent(byte[] content)
            {
                m_Content = content;
            }
        }

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
