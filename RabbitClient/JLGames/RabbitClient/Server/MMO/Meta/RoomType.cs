namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Room lifecycle category.
    /// 房间生命周期类别。
    /// </summary>
    public enum RoomType : byte
    {
        /// <summary>
        /// Undefined room type.
        /// 未定义
        /// </summary>
        None,

        /// <summary>
        /// Standard persistent room.
        /// 常规
        /// </summary>
        Normal,

        /// <summary>
        /// Temporary room.
        /// 临时
        /// </summary>
        Temp,
    }
}
