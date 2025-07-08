using System;
using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    public interface IEntityRoom : IEntity, IEquatable<IEntityRoom>, IEventDispatcher, IUpdateSupport, IVarSupport
    {
        /// <summary>
        /// 房间Id
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// 房间名称
        /// </summary>
        string RoomName { get; }

        /// <summary>
        /// 玩家数量
        /// </summary>
        int PlayerCount { get; }

        /// <summary>
        /// 单位数量
        /// </summary>
        int UnitCount { get; }

        /// <summary>
        /// 查找玩家信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        IEntityPlayer FindIPlayer(string playerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        IEntityUnit FindIUnit(string unitId);

        /// <summary>
        /// 遍历每个玩家
        /// </summary>
        /// <param name="each"></param>
        void ForEachPlayer(Action<int, IEntityPlayer> each);

        /// <summary>
        /// 遍历每个单位
        /// </summary>
        /// <param name="each"></param>
        void ForEachUnit(Action<int, IEntityUnit> each);
    }
}