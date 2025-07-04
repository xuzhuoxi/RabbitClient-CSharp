
using JLGames.Infra;

namespace JLGames.RabbitClient.Server
{
    public interface IRabbitResponseMsg : IRabbitMessageReader, ICloneable<IRabbitResponseMsg>
    {
        /// <summary>
        /// Response Result Code
        /// 响应结构状态码
        /// </summary>
        int RsCode { get; }
    }
}