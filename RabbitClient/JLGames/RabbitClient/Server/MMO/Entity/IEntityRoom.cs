using System;
using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Room entity containing players and units.
    /// 房间实体，包含玩家与单位。
    /// </summary>
    public interface IEntityRoom : IEntity, IEquatable<IEntityRoom>, IEventDispatcher, IUpdateSupport, IVarSupport
    {
        /// <summary>
        /// Room identifier.
        /// 房间Id
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// Display name of the room.
        /// 房间名称
        /// </summary>
        string RoomName { get; }

        /// <summary>
        /// Number of players in the room.
        /// 玩家数量
        /// </summary>
        int PlayerCount { get; }

        /// <summary>
        /// Number of units in the room.
        /// 单位数量
        /// </summary>
        int UnitCount { get; }

        /// <summary>
        /// Finds a player by id.
        /// 查找玩家信息
        /// </summary>
        /// <param name="playerId">Player id<br/>玩家Id</param>
        /// <returns>Player entity or null<br/>玩家实体或 null</returns>
        IEntityPlayer FindIPlayer(string playerId);

        /// <summary>
        /// Finds a unit by id.
        /// 按单位Id查找单位
        /// </summary>
        /// <param name="unitId">Unit id<br/>单位Id</param>
        /// <returns>Unit entity or null<br/>单位实体或 null</returns>
        IEntityUnit FindIUnit(string unitId);

        /// <summary>
        /// Iterates all players in the room.
        /// 遍历每个玩家
        /// </summary>
        /// <param name="each">Callback with index and player<br/>带索引与玩家的回调</param>
        void ForEachPlayer(Action<int, IEntityPlayer> each);

        /// <summary>
        /// Iterates all units in the room.
        /// 遍历每个单位
        /// </summary>
        /// <param name="each">Callback with index and unit<br/>带索引与单位的回调</param>
        void ForEachUnit(Action<int, IEntityUnit> each);
    }
}
