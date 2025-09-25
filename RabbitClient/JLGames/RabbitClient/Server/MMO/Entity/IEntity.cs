using System;

namespace JLGames.RabbitClient.Server.MMO
{
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