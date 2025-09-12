using System;

namespace JLGames.RabbitClient.Server.MMO
{
    [Flags]
    public enum EntityType
    {
        EntityUnit = 1 << 0,
        EntityPlayer = 1 << 1,
        EntityRoom = 1 << 2,
        EntityTeam = 1 << 3,
        EntityTeamCorps = 1 << 4,
        EntityChannel = 1 << 5,
    }

    public static class EntityTypeUtil
    {
        public static EntityType EntityNone = 0;

        public static EntityType EntityAll = EntityType.EntityUnit | EntityType.EntityPlayer | EntityType.EntityRoom | EntityType.EntityTeam |
                                             EntityType.EntityTeamCorps | EntityType.EntityChannel;

        /// <summary>
        /// check where 'self' is a part of 'check'
        /// 检查self是否为check中的一部分
        /// </summary>
        /// <param name="self"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool Match(this EntityType self, EntityType check)
        {
            return (self & check) > 0;
        }

        /// <summary>
        /// Check if self contains check
        /// 检查self是否包含check
        /// </summary>
        /// <param name="self"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool Include(this EntityType self, EntityType check)
        {
            return (self & check) == check;
        }
    }
}