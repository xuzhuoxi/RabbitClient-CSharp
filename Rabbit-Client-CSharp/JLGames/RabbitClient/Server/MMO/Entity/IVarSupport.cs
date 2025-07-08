namespace JLGames.RabbitClient.Server.MMO
{
    public interface IVarSupport
    {
        /// <summary>
        /// 变量
        /// </summary>
        IVarSet VarSet { get; }

        /// <summary>
        /// Set local var
        /// 设置本地属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetVar(string key, object value);

        /// <summary>
        /// Set local vars
        /// 设置本地属性
        /// </summary>
        /// <param name="vars"></param>
        void SetVars(IVarSet vars);

        /// <summary>
        /// Del local var
        /// 删除本地属性
        /// </summary>
        /// <param name="key"></param>
        void DelVar(string key);

        /// <summary>
        /// Del local vars
        /// 删除本地属性
        /// </summary>
        /// <param name="keys"></param>
        void DelVars(string[] keys);
    }
}