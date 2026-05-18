using System.Collections.Generic;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Key-value variable set with timestamps and network encoding.
    /// 带时间戳与网络编码的键值变量集合。
    /// </summary>
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
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Timestamp key name<br/>时间戳键名</returns>
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
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="value">Basic data types and their array types; V3Int; V2Int<br/>基础类型及其数组；V3Int；V2Int</param>
        void SetVar(string key, object value);

        /// <summary>
        /// Set var value.
        /// 设置变量值
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="value">Basic data types and their array types; V3Int; V2Int<br/>基础类型及其数组；V3Int；V2Int</param>
        /// <param name="timestamp">Value timestamp in ticks<br/>变量值时间戳（Ticks）</param>
        void SetVar(string key, object value, long timestamp);

        /// <summary>
        /// Remove and return var value.
        /// 删除变量值，并返回
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <param name="includeStampKey">Whether to remove timestamp key<br/>是否同时删除时间戳键</param>
        /// <returns>Removed value or null<br/>被删除的值或 null</returns>
        object DeleteVar(string key, bool includeStampKey);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="vars">Key-value dictionary<br/>键值字典</param>
        void SetVars(Dictionary<string, object> vars);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="vars">Key-value dictionary<br/>键值字典</param>
        /// <param name="timestamp">Shared timestamp in ticks<br/>统一时间戳（Ticks）</param>
        void SetVars(Dictionary<string, object> vars, long timestamp);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="set">Source variable set<br/>源变量集合</param>
        void SetVars(IVarSet set);

        /// <summary>
        /// set var values in batch
        /// 批量设置变量值
        /// </summary>
        /// <param name="set">Source variable set<br/>源变量集合</param>
        /// <param name="timestamp">Shared timestamp in ticks<br/>统一时间戳（Ticks）</param>
        void SetVars(IVarSet set, long timestamp);

        /// <summary>
        /// Remove and return var value.
        /// 删除变量值，并返回
        /// </summary>
        /// <param name="keys">Variable keys<br/>变量键数组</param>
        /// <param name="includeStampKey">Whether to remove timestamp keys<br/>是否同时删除时间戳键</param>
        void DeleteVars(string[] keys, bool includeStampKey);

        /// <summary>
        /// check key is exist or not
        /// 检查是否包含key
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>True if key exists<br/>键存在返回 true</returns>
        bool CheckKey(string key);

        /// <summary>
        /// get value
        /// 获取值
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Stored value or null<br/>存储的值或 null</returns>
        object GetValue(string key);

        /// <summary>
        /// get value
        /// 获取值
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <typeparam name="T">Basic data types and their array types; V3Int; V2Int<br/>基础类型及其数组；V3Int；V2Int</typeparam>
        /// <returns>Typed value or default<br/>类型化值或默认值</returns>
        T GetValue<T>(string key);

        /// <summary>
        /// get value timestamp
        /// 获取值设置时的时间戳
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Timestamp in ticks, or 0<br/>时间戳（Ticks），无则返回 0</returns>
        long GetValueStamp(string key);

        /// <summary>
        /// iteration
        /// 遍历
        /// </summary>
        /// <param name="each">Per key-value callback<br/>键值对回调</param>
        void ForEach(VarSetDelegates.FuncEach each);

        /// <summary>
        /// Iterates each variable with its timestamp.
        /// 遍历每个变量及其时间戳
        /// </summary>
        /// <param name="stampEach">Per key-value-timestamp callback<br/>键值时间戳回调</param>
        void ForEach(VarSetDelegates.FuncStampEach stampEach);
    }
}
