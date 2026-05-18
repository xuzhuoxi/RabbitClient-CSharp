using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit message writer with header and payload operations.
    /// Rabbit消息写入接口，包含消息头与载荷操作。
    /// </summary>
    public interface IRabbitMessageWriter : IRabbitMessageHeader
    {
        /// <summary>
        /// Set the message header, and write it to the buff.
        /// 重新设置消息表头, 并写入到缓存
        /// </summary>
        void WriteHeader();

        /// <summary>
        /// Set the message header, and write it to the buff.
        /// 重新设置消息表头, 并写入到缓存
        /// </summary>
        /// <param name="extension">Extension name<br/>扩展名</param>
        /// <param name="protoId">Protocol identifier<br/>协议Id</param>
        /// <param name="uid">Client identifier<br/>客户端标识</param>
        void WriteHeader(string extension, string protoId, string uid);

        /// <summary>
        /// Write object data that implements the INetMessage interface
        /// 写入实现了INetMessage接口的对象数据
        /// </summary>
        /// <param name="msg">Message to encode and write<br/>要编码并写入的消息对象</param>
        void WriteMessage(INetMessage msg);

        /// <summary>
        /// write data
        /// 写入数据
        /// Supports the implementation object that supports INetMessage
        /// 支持INetMessage的实现对象
        /// Supports basic data types and their arrays
        /// 支持基础数据类型及其数组
        /// </summary>
        /// <param name="data">Value to write<br/>要写入的数据</param>
        void WriteData(object data);

        /// <summary>
        /// Read message bytes.
        /// 读取消息字节数据
        /// </summary>
        /// <returns>Complete encoded message bytes<br/>完整编码后的消息字节</returns>
        byte[] ToMessageBytes();
    }
}
