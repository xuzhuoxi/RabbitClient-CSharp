namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Persistence semantics for MMO variables.
    /// MMO 变量的持久化语义。
    /// </summary>
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
