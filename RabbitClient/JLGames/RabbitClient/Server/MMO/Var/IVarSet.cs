using System.Collections.Generic;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.MMO
{
    public interface IVarSet : INetMessage
    {
        /// <summary>
        /// all var size
        /// 全部变量数量
        /// </summary>
        int Size { get; }

        /// <summary>
        /// key var size
        /// 变量数量
        /// </summary>
        int KeySize { get; }

        /// <summary>
        /// key name to stamp key name
        /// 变量键转变量时间戳键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string KeyToStampKey(string key);

        /// <summary>
        /// Clear the var set.
        /// 清理变量集合
        /// </summary>
        void Clear();

        /// <summary>
        /// Set var value.
        /// 设置变量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetVar(string key, object value);

        /// <summary>
        /// Set var value.
        /// 设置变量值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timestamp"></param>
        void SetVar(string key, object value, long timestamp);

        /// <summary>
        /// Remove and return var value.
        /// 删除变量值，并返回
        /// </summary>
        /// <param name="key"></param>
        /// <param name="includeStampKey">包含时间</param>
        /// <returns></returns>
        object DeleteVar(string key, bool includeStampKey);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="vars"></param>
        void SetVars(Dictionary<string, object> vars);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="timestamp"></param>
        void SetVars(Dictionary<string, object> vars, long timestamp);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="set"></param>
        void SetVars(IVarSet set);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="timestamp"></param>
        void SetVars(IVarSet set, long timestamp);

        /// <summary>
        /// Remove and return var value.
        /// 删除变量值，并返回
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includeStampKey"></param>
        /// <returns></returns>
        void DeleteVars(string[] keys, bool includeStampKey);

        /// <summary>
        /// check key is exist or not
        /// 检查是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckKey(string key);

        /// <summary>
        /// get value
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetValue(string key);

        /// <summary>
        /// get value
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// get value timestamp
        /// 获取值设置时的时间戳
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long GetValueStamp(string key);

        /// <summary>
        /// iteration
        /// 遍历
        /// </summary>
        /// <param name="each"></param>
        void ForEach(VarSetDelegates.FuncEach each);

        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="stampEach"></param>
        void ForEach(VarSetDelegates.FuncStampEach stampEach);
    }
}