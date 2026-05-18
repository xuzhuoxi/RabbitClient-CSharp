using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Updates entity state from a network response reader.
    /// 从网络响应读取器更新实体状态。
    /// </summary>
    public interface IUpdateSupport
    {
        /// <summary>
        /// Update data from a IRabbitResponseMsg object.
        /// 从一个 IRabbitResponseMsg 对象中更新数据
        /// </summary>
        /// <param name="reader">Response message reader<br/>响应消息读取器</param>
        void UpdateFromReader(IRabbitResponseMsg reader);
    }
}
