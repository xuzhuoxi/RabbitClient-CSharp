using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
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
        /// <param name="extension"></param>
        /// <param name="protoId"></param>
        /// <param name="uid"></param>
        void WriteHeader(string extension, string protoId, string uid);

        /// <summary>
        /// Write object data that implements the INetMessage interface
        /// 写入实现了INetMessage接口的对象数据
        /// </summary>
        /// <param name="msg"></param>
        void WriteMessage(INetMessage msg);

        /// <summary>
        /// write data
        /// 写入数据
        /// Supports the implementation object that supports INetMessage
        /// 支持INetMessage的实现对象
        /// Supports basic data types and their arrays
        /// 支持基础数据类型及其数组
        /// </summary>
        /// <param name="data"></param>
        void WriteData(object data);

        /// <summary>
        /// Read message bytes.
        /// 读取消息字节数据
        /// </summary>
        /// <returns></returns>
        byte[] ToMessageBytes();
    }
}
