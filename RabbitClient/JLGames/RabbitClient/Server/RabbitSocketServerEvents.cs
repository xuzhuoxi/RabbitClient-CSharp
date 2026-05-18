namespace JLGames.RabbitClient.Server
{
    /// <summary>
    /// Event names dispatched by RabbitSocketServer.
    /// RabbitSocketServer 分发的事件名。
    /// </summary>
    public static class RabbitSocketServerEvents
    {
        /// <summary>
        /// Enable connection success event
        /// 连接成功事件
        /// Event data(事件数据)：SocketEvents.SocketConnEventInfo
        /// </summary>
        public const string EventOnConnectionOpenSuc = "RabbitSocketServerEvents.EventOnConnectionOpenSuc";
        /// <summary>
        /// Enable connection fail event
        /// 连接失败事件
        /// Event data(事件数据)：SocketEvents.SocketConnEventInfo
        /// </summary>
        public const string EventOnConnectionOpenFail = "RabbitSocketServerEvents.EventOnConnectionOpenFail";
        
        /// <summary>
        /// Close connection result event
        /// 关闭连接结果事件
        /// Event data(事件数据)：SocketConnEventInfo
        /// </summary>
        public const string EventOnConnectionClose = "RabbitSocketServerEvents.EventOnConnectClose";
        
        /// <summary>
        /// Data reception processing completed
        /// 数据接收处理结束
        /// Event data(事件数据)：byte[] message
        /// </summary>
        public const string EventOnServerMessage = "RabbitSocketServerEvents.EventOnServerMessage";
    }
}
