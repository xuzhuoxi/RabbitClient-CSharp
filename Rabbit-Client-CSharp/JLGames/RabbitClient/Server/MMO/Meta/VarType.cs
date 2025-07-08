namespace JLGames.RabbitClient.Server.MMO
{
    public enum VarType : byte
    {
        /// <summary>
        /// Undefined
        /// 未定义
        /// </summary>
        Undefined,

        /// <summary>
        /// Moment
        /// 瞬间
        /// </summary>
        Moment,

        /// <summary>
        /// Forever until overwritten.
        /// 永久直到被覆盖
        /// </summary>
        Forever,

        /// <summary>
        /// keep some time
        /// 持续一段时间
        /// </summary>
        Duration,
    }
}