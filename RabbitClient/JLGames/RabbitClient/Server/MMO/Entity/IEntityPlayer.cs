using System;
using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    public interface IEntityPlayer : IEntity, IEquatable<IEntityPlayer>, IEventDispatcher,
        IVarSupport, IPosSupport, ITowardSupport, IInputSupport, IUpdateSupport
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        string PlayerId { get; }

        /// <summary>
        /// Is it the current player.
        /// 是否为当前玩家
        /// </summary>
        bool IsSelf { get; }

        /// <summary>
        /// Set self player id.
        /// 设置当前玩家Id.
        /// </summary>
        /// <param name="selfId"></param>
        void SetSelfPlayerId(string selfId);


        // 以下为扩展属性 ---------- ---------- ---------- ----------

        /// <summary>
        /// Player nick name
        /// 玩家昵称
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// Team Id
        /// 团队Id
        /// </summary>
        string TeamId { get; }
    }
}