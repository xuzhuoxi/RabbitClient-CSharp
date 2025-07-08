using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    public interface IUpdateSupport
    {
        /// <summary>
        /// Update data from a IRabbitResponseMsg object.
        /// 从一个 IRabbitResponseMsg 对象中更新数据
        /// </summary>
        /// <param name="reader"></param>
        void UpdateFromReader(IRabbitResponseMsg reader);
    }
}