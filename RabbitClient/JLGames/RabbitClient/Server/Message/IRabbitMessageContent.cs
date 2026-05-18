namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit message raw content accessors.
    /// Rabbit消息原始内容访问接口。
    /// </summary>
    public interface IRabbitMessageContent : IRabbitMessageHeader
    {
        /// <summary>
        /// Message content
        /// 消息内容
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// Message string content
        /// 消息字符串内容
        /// </summary>
        string ContentString { get; }
    }
}
