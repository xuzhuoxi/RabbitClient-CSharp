using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    public interface IRabbitRequestMsg : IRabbitMessageWriter
    {
        /// <summary>
        /// Set response info
        /// 设置响应信息
        /// </summary>
        /// <param name="extName"></param>
        /// <param name="protoId"></param>
        void SetProtoInfo(string extName, string protoId);
        
        /// <summary>
        /// Set client id
        /// 设置客户端Id
        /// </summary>
        /// <param name="cid"></param>
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
        /// <param name="baseValue"></param>
        void WriteRequestBase(object baseValue);
        
        /// <summary>
        /// Write message data
        /// 写入消息数据
        /// </summary>
        /// <param name="reqMsg"></param>
        void WriteRequestMessage(INetMessage reqMsg);
    }
}