using JLGames.Infra.Buffer;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit message reader with header and buffer operations.
    /// Rabbit消息读取接口，包含消息头与缓冲区操作。
    /// </summary>
    public interface IRabbitMessageReader : IRabbitMessageHeader, IDataBufferReader, IDataBufferCopier, IByteBufferReader, IByteBufferCopier
    {
        /// <summary>
        /// Is there any data left to read
        /// 是否还有数据未读取
        /// </summary>
        bool Next { get; }

        /// <summary>
        /// Start to read message body data
        /// 开始读取消息内容数据
        /// </summary>
        void StartReadData();

        /// <summary>
        /// Reads an INetMessage object from the buffer and decodes it into the target.
        /// 读取实现了INetMessage接口的对象数据并解码到目标对象。
        /// </summary>
        /// <param name="message">Target message to decode into<br/>解码目标消息对象</param>
        void ReadMessageTo(INetMessage message);

        /// <summary>
        /// Copies an INetMessage payload from the buffer without advancing the read position.
        /// 读取实现了INetMessage接口的对象数据，不移动读下标。
        /// </summary>
        /// <param name="message">Target message to decode into<br/>解码目标消息对象</param>
        void CopyMessageTo(INetMessage message);

        /// <summary>
        /// Read the data and save it to data, and match it according to the data type of data
        /// 读取数据并保存到data中，根据data的数据类型进行匹配
        /// Supports the implementation object that supports INetMessage
        /// 支持INetMessage的实现对象
        /// Supports basic data types and their arrays
        /// 支持基础数据类型及其数组
        /// </summary>
        /// <param name="data">Object reference to receive the read value<br/>用于接收读取值的对象引用</param>
        void ReadDataTo(ref object data);

        /// <summary>
        /// Read the data and save it to data, and match it according to the data type of data
        /// 读取数据并保存到data中，根据data的数据类型进行匹配
        /// Supports the implementation object that supports INetMessage
        /// 支持INetMessage的实现对象
        /// Supports basic data types and their arrays
        /// 支持基础数据类型及其数组
        /// Note: not move reader index
        /// 注意：不移动读下标
        /// </summary>
        /// <param name="data">Object reference to receive the copied value<br/>用于接收复制值的对象引用</param>
        void CopyDataTo(ref object data);

        /// <summary>
        /// Copy remain bytes.
        /// 复制剩余字节
        /// </summary>
        /// <returns>Remaining unread bytes<br/>剩余未读字节</returns>
        byte[] CopyRemains();

        /// <summary>
        /// Update the current object's data with byte data
        /// 使用字节数据更新当前对象的数据
        /// </summary>
        /// <param name="msg">Raw message bytes<br/>原始消息字节</param>
        void SetMessageBytes(byte[] msg);
    }
}
