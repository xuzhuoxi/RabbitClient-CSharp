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
    }
}