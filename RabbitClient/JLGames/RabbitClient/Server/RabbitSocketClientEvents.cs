using System;

namespace JLGames.RabbitClient.Server
{
    public static class RabbitSocketClientEvents
    {
        /// <summary>
        /// Data reception processing completed
        /// 数据接收处理结束
        /// Event data(事件数据)：IRabbitResponseMsg
        /// </summary>
        public const string EventOnClientMessage = "RabbitSocketClientEvents.EventOnClientMessage";

        /// <summary>
        /// Send or Receive Data Failed event
        /// 收发消息失败事件
        /// Event data(事件数据): FailedInfo
        /// </summary>
        public const string EventOnClientMessageFailed = "RabbitSocketClientEvents.EventOnClientMessageFailed";

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
