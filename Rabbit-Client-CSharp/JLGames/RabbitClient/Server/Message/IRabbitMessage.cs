namespace JLGames.RabbitClient.Server.Message
{
    public interface IRabbitMessage
    {
        /// <summary>
        /// Extension Name
        /// 扩展名
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Proto Id
        /// 协议Id
        /// </summary>
        string ProtoId { get; }

        /// <summary>
        /// Message client ID
        /// 消息客户端标识
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// ProtoUUID
        /// 协议唯一Id
        /// </summary>
        string ProtoUUID { get; }
    }
}