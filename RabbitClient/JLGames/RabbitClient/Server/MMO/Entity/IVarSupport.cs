namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Entity-local variable get/set support.
    /// 实体本地变量的读写支持。
    /// </summary>
    public interface IVarSupport
    {
        /// <summary>
        /// Variable set attached to this entity.
        /// 变量
        /// </summary>
        IVarSet VarSet { get; }

        /// <summary>
        /// Set local var
        /// 设置本地属性
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="value">Variable value<br/>变量值</param>
        void SetVar(string key, object value);

        /// <summary>
        /// Set local vars
        /// 设置本地属性
        /// </summary>
        /// <param name="vars">Variable set to merge<br/>要合并的变量集合</param>
        void SetVars(IVarSet vars);

        /// <summary>
        /// Del local var
        /// 删除本地属性
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        void DelVar(string key);

        /// <summary>
        /// Del local vars
        /// 删除本地属性
        /// </summary>
        /// <param name="keys">Variable keys<br/>变量键数组</param>
        void DelVars(string[] keys);
    }
}
