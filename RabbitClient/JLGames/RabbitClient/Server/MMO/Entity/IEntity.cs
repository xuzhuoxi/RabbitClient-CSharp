using System;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Base contract for all MMO entities.
    /// 所有 MMO 实体的基础契约。
    /// </summary>
    public interface IEntity : IEquatable<IEntity>
    {
        /// <summary>
        /// Entity type
        /// 实体类型
        /// </summary>
        EntityType EntityType { get; }

        /// <summary>
        /// Entity id
        /// 实体Id
        /// </summary>
        string EntityId { get; }
    }
}
