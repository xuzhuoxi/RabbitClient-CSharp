using System;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Bit flags identifying MMO entity kinds.
    /// 标识 MMO 实体种类的位标志。
    /// </summary>
    [Flags]
    public enum EntityType
    {
        /// <summary>
        /// World unit entity.
        /// 世界单位实体
        /// </summary>
        EntityUnit = 1 << 0,

        /// <summary>
        /// Player entity.
        /// 玩家实体
        /// </summary>
        EntityPlayer = 1 << 1,

        /// <summary>
        /// Room entity.
        /// 房间实体
        /// </summary>
        EntityRoom = 1 << 2,

        /// <summary>
        /// Team entity.
        /// 队伍实体
        /// </summary>
        EntityTeam = 1 << 3,

        /// <summary>
        /// Team corps entity.
        /// 军团实体
        /// </summary>
        EntityTeamCorps = 1 << 4,

        /// <summary>
        /// Channel entity.
        /// 频道实体
        /// </summary>
        EntityChannel = 1 << 5,
    }

    /// <summary>
    /// Extension methods and constants for EntityType flags.
    /// EntityType 标志的扩展方法与常量。
    /// </summary>
    public static class EntityTypeUtil
    {
        /// <summary>
        /// Empty entity type mask.
        /// 空实体类型掩码
        /// </summary>
        public static EntityType EntityNone = 0;

        /// <summary>
        /// Mask including all defined entity types.
        /// 包含所有已定义实体类型的掩码
        /// </summary>
        public static EntityType EntityAll = EntityType.EntityUnit | EntityType.EntityPlayer | EntityType.EntityRoom | EntityType.EntityTeam |
                                             EntityType.EntityTeamCorps | EntityType.EntityChannel;

        /// <summary>
        /// check where 'self' is a part of 'check'
        /// 检查self是否为check中的一部分
        /// </summary>
        /// <param name="self">Source flags<br/>源标志</param>
        /// <param name="check">Flags to test against<br/>待检测标志</param>
        /// <returns>True if any bit overlaps<br/>有任意位重叠返回 true</returns>
        public static bool Match(this EntityType self, EntityType check)
        {
            return (self & check) > 0;
        }

        /// <summary>
        /// Check if self contains check
        /// 检查self是否包含check
        /// </summary>
        /// <param name="self">Source flags<br/>源标志</param>
        /// <param name="check">Required flags<br/>需包含的标志</param>
        /// <returns>True if all check bits are set<br/>check 全部位已设置返回 true</returns>
        public static bool Include(this EntityType self, EntityType check)
        {
            return (self & check) == check;
        }
    }
}
