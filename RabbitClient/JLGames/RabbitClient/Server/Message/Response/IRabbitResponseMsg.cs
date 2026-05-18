using JLGames.Infra;

namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit server response message reader.
    /// Rabbit服务端响应消息读取接口。
    /// </summary>
    public interface IRabbitResponseMsg : IRabbitMessageReader, ICloneable<IRabbitResponseMsg>
    {
        /// <summary>
        /// Response Result Code
        /// 响应结构状态码
        /// </summary>
        int RsCode { get; }
    }
}
