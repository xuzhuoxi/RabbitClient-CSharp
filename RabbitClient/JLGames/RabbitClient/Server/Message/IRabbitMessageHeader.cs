namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Common Rabbit message header properties.
    /// Rabbit消息头通用属性。
    /// </summary>
    public interface IRabbitMessageHeader
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
        /// ProtoUID
        /// 协议唯一Id
        /// </summary>
        string ProtoUid { get; }
    }
}
