namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Category of world unit entities.
    /// 世界单位实体的类别。
    /// </summary>
    public enum UnitType : byte
    {
        /// <summary>
        /// Undefined unit type.
        /// 未定义
        /// </summary>
        None,

        /// <summary>
        /// Building unit.
        /// 建筑
        /// </summary>
        Building,

        /// <summary>
        /// Troop unit.
        /// 部队
        /// </summary>
        Troop,

        /// <summary>
        /// Banner or flag unit.
        /// 旗帜
        /// </summary>
        Banner,
    }
}
