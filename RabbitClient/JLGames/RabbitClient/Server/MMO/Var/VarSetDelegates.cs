namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Delegate types used when iterating variable sets.
    /// 遍历变量集合时使用的委托类型。
    /// </summary>
    public static class VarSetDelegates
    {
        /// <summary>
        /// Invoked for each key-value pair in a variable set.
        /// 遍历变量集合时，对每个键值对调用。
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="value">Variable value<br/>变量值</param>
        public delegate void FuncEach(string key, object value);

        /// <summary>
        /// Invoked for each key-value pair including its timestamp.
        /// 遍历变量集合时，对每个键值对及其时间戳调用。
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="value">Variable value<br/>变量值</param>
        /// <param name="stamp">Value timestamp in ticks<br/>变量值时间戳（Ticks）</param>
        public delegate void FuncStampEach(string key, object value, long stamp);
    }
}
