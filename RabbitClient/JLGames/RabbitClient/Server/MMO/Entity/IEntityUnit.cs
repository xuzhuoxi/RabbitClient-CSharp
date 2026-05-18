using System;
using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Unit entity in a room with owner and variable support.
    /// 房间内单位实体，支持拥有者与变量。
    /// </summary>
    public interface IEntityUnit : IEntity, IEquatable<IEntityUnit>, IEventDispatcher,
        IVarSupport, IPosSupport, ITowardSupport, IInputSupport, IUpdateSupport
    {
        /// <summary>
        /// Unit identifier.
        /// 单位Id
        /// </summary>
        string UnitId { get; }

        /// <summary>
        /// Owner player or entity id.
        /// 拥有者
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// Room id the unit belongs to.
        /// 所在房间Id
        /// </summary>
        string RoomId { get; }
    }
}
