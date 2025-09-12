namespace JLGames.RabbitClient
{
    public static class RabbitClientManagerEvents
    {
        public class ProgressEventData<T>
        {
            public bool Suc { get; internal set; }
            public T Data { get; internal set; }

            public override string ToString()
            {
                return
                    $"ProgressEventData<{Data.GetType().Name}>{{Suc={Suc}, Data={Data}}}";
            }
        }

        /// <summary>
        /// Connect progress event
        /// 连接进度事件
        /// Event data(事件数据)：<![CDATA[ProgressEventData<QueryResult>]]>
        /// </summary>
        public const string EventOnProgressHome = "RabbitClientManagerEvents.EventOnProgressHome";

        /// <summary>
        /// Connect progress event
        /// 连接进度事件
        /// Event data(事件数据)：<![CDATA[ProgressEventData<SocketEvents.SocketConnEventInfo>]]>
        /// </summary>
        public const string EventOnProgressServer = "RabbitClientManagerEvents.EventOnProgressServer";

        /// <summary>
        /// Connect success event
        /// 连接成功事件
        /// Event data(事件数据)：null
        /// </summary>
        public const string EventOnConnectFinish = "RabbitClientManagerEvents.EventOnLinkSuc";
    }
}