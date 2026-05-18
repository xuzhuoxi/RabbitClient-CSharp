using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit client request message writer.
    /// Rabbit客户端请求消息写入接口。
    /// </summary>
    public interface IRabbitRequestMsg : IRabbitMessageWriter
    {
        /// <summary>
        /// Set request protocol info
        /// 设置请求协议信息
        /// </summary>
        /// <param name="extName">Extension name<br/>扩展名</param>
        /// <param name="protoId">Protocol identifier<br/>协议Id</param>
        void SetProtoInfo(string extName, string protoId);
        
        /// <summary>
        /// Set client id
        /// 设置客户端Id
        /// </summary>
        /// <param name="cid">Client identifier<br/>客户端标识</param>
        void SetClientId(string cid);

        /// <summary>
        /// Start write data
        /// 开始写入数据
        /// </summary>
        void StartWriteData();

        /// <summary>
        /// Write base type data
        /// 写入基数类型数据
        /// </summary>
        /// <param name="baseValue">Primitive or supported value to write<br/>要写入的基础类型或支持的值</param>
        void WriteRequestBase(object baseValue);
        
        /// <summary>
        /// Write message data
        /// 写入消息数据
        /// </summary>
        /// <param name="reqMsg">Request message to encode and write<br/>要编码并写入的请求消息</param>
        void WriteRequestMessage(INetMessage reqMsg);
    }
}
