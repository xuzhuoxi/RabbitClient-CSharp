using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit client request message builder.
    /// Rabbit客户端请求消息构建器。
    /// </summary>
    public class RabbitRequestMsg : RabbitMessageWriter, IRabbitRequestMsg
    {
        private readonly bool m_LittleEndian;

        /// <summary>
        /// Creates a request message writer with optional little-endian byte order.
        /// 创建请求消息写入器，可选小端字节序。
        /// </summary>
        /// <param name="littleEndian">Whether to use little-endian encoding<br/>是否使用小端编码</param>
        public RabbitRequestMsg(bool littleEndian = true) : base(littleEndian)
        {
            m_LittleEndian = littleEndian;
        }

        /// <summary>
        /// Sets the client identifier on the message header.
        /// 设置消息头中的客户端标识。
        /// </summary>
        /// <param name="cid">Client identifier<br/>客户端标识</param>
        public void SetClientId(string cid)
        {
            m_MessageHeader.ClientId = cid;
        }

        /// <summary>
        /// Sets extension and protocol id on the message header.
        /// 设置消息头中的扩展名与协议Id。
        /// </summary>
        /// <param name="extName">Extension name<br/>扩展名</param>
        /// <param name="protoId">Protocol identifier<br/>协议Id</param>
        public void SetProtoInfo(string extName, string protoId)
        {
            m_MessageHeader.Extension = extName;
            m_MessageHeader.ProtoId = protoId;
        }

        /// <summary>
        /// Writes the message header to start the request body.
        /// 写入消息头以开始请求体。
        /// </summary>
        public void StartWriteData()
        {
            WriteHeader();
        }

        /// <summary>
        /// Writes a base-type request field if not null.
        /// 写入基础类型请求字段（非空时）。
        /// </summary>
        /// <param name="baseValue">Primitive or supported value<br/>基础类型或支持的值</param>
        public void WriteRequestBase(object baseValue)
        {
            if (null == baseValue) return;
            WriteData(baseValue);
        }

        /// <summary>
        /// Writes an INetMessage request payload if not null.
        /// 写入INetMessage请求载荷（非空时）。
        /// </summary>
        /// <param name="reqMsg">Request message to encode<br/>要编码的请求消息</param>
        public void WriteRequestMessage(INetMessage reqMsg)
        {
            if (null == reqMsg) return;
            WriteMessage(reqMsg);
        }
    }
}
