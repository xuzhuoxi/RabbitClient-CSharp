using System;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Default implementation of Rabbit message writing.
    /// Rabbit消息写入默认实现。
    /// </summary>
    public class RabbitMessageWriter : IRabbitMessageWriter
    {
        internal RabbitMessageHeader m_MessageHeader;
        protected readonly INetMessageWriter m_Packer;

        /// <summary>
        /// Extension name from the message header.
        /// 消息头中的扩展名。
        /// </summary>
        public string Extension => m_MessageHeader.Extension;

        /// <summary>
        /// Protocol identifier from the message header.
        /// 消息头中的协议Id。
        /// </summary>
        public string ProtoId => m_MessageHeader.ProtoId;

        /// <summary>
        /// Client identifier from the message header.
        /// 消息头中的客户端标识。
        /// </summary>
        public string ClientId => m_MessageHeader.ClientId;

        /// <summary>
        /// Unique protocol id from the message header.
        /// 消息头中的协议唯一Id。
        /// </summary>
        public string ProtoUid => m_MessageHeader.ProtoUid;

        /// <summary>
        /// Creates a message writer with optional little-endian byte order.
        /// 创建消息写入器，可选小端字节序。
        /// </summary>
        /// <param name="littleEndian">Whether to use little-endian encoding<br/>是否使用小端编码</param>
        public RabbitMessageWriter(bool littleEndian = true)
        {
            m_MessageHeader = new RabbitMessageHeader();
            m_Packer = new NetMessageWriter(littleEndian);
        }

        /// <summary>
        /// Clears the buffer and writes the current header fields.
        /// 清空缓冲区并写入当前消息头字段。
        /// </summary>
        public void WriteHeader()
        {
            m_Packer.Clear();
            m_Packer.WriteData(m_MessageHeader.Extension);
            m_Packer.WriteData(m_MessageHeader.ProtoId);
            m_Packer.WriteData(m_MessageHeader.ClientId);
        }

        /// <summary>
        /// Sets header fields and writes them to the buffer.
        /// 设置消息头字段并写入缓冲区。
        /// </summary>
        /// <param name="extension">Extension name<br/>扩展名</param>
        /// <param name="protoId">Protocol identifier<br/>协议Id</param>
        /// <param name="cid">Client identifier<br/>客户端标识</param>
        public void WriteHeader(string extension, string protoId, string cid)
        {
            m_MessageHeader.SetHeaderInfo(extension, protoId, cid);
            WriteHeader();
        }

        /// <summary>
        /// Encodes and writes an INetMessage payload.
        /// 编码并写入INetMessage载荷。
        /// </summary>
        /// <param name="msg">Message to encode<br/>要编码的消息</param>
        public void WriteMessage(INetMessage msg)
        {
            m_Packer.WriteData(msg.EncodeToBytes());
        }

        /// <summary>
        /// Writes supported data types or INetMessage objects.
        /// 写入支持的数据类型或INetMessage对象。
        /// </summary>
        /// <param name="data">Value to write<br/>要写入的数据</param>
        public void WriteData(object data)
        {
            if (data is INetMessage message)
            {
                m_Packer.WriteMessage(message);
                return;
            }

            try
            {
                m_Packer.WriteBaseData(data);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Returns the complete encoded message bytes.
        /// 返回完整编码后的消息字节。
        /// </summary>
        /// <returns>Encoded message bytes<br/>编码后的消息字节</returns>
        public byte[] ToMessageBytes()
        {
            return m_Packer.ReadMessageBytes();
        }
    }
}
