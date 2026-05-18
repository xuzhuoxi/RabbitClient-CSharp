namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit protocol message header fields.
    /// Rabbit协议消息头字段。
    /// </summary>
    public struct RabbitMessageHeader
    {
        /// <summary>
        /// Extension name.
        /// 扩展名。
        /// </summary>
        public string Extension;

        /// <summary>
        /// Protocol identifier.
        /// 协议Id。
        /// </summary>
        public string ProtoId;

        /// <summary>
        /// Client identifier.
        /// 客户端标识。
        /// </summary>
        public string ClientId;

        /// <summary>
        /// Unique protocol id composed of extension and proto id.
        /// 由扩展名与协议Id组成的唯一协议标识。
        /// </summary>
        public string ProtoUid => $"{Extension}:{ProtoId}";

        /// <summary>
        /// Sets all header field values.
        /// 设置消息头各字段值。
        /// </summary>
        /// <param name="extension">Extension name<br/>扩展名</param>
        /// <param name="protoId">Protocol identifier<br/>协议Id</param>
        /// <param name="cid">Client identifier<br/>客户端标识</param>
        public void SetHeaderInfo(string extension, string protoId, string cid)
        {
            Extension = extension;
            ProtoId = protoId;
            ClientId = cid;
        }
    }
}
