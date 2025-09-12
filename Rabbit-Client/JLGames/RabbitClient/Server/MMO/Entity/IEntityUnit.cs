using System;
using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    public interface IEntityUnit : IEntity, IEquatable<IEntityUnit>, IEventDispatcher,
        IVarSupport, IPosSupport, ITowardSupport, IInputSupport, IUpdateSupport
    {
        /// <summary>
        /// 单位Id
        /// </summary>
        string UnitId { get; }

        /// <summary>
        /// 拥有者
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// 所在房间Id
        /// </summary>
        string RoomId { get; }
    }
}