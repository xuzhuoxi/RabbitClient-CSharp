using System.Collections.Generic;
using JLGames.Infra.Buffer;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Default implementation of a network-encoded variable set.
    /// 可网络编码的变量集合默认实现。
    /// </summary>
    public class VarSet : IVarSet
    {
        private readonly CodingMap m_Map;
        private readonly HashSet<string> m_KeySet;
        private readonly HashSet<string> m_TempKeySet;

        /// <summary>
        /// Returns a string representation of the underlying map.
        /// 返回底层映射的字符串表示。
        /// </summary>
        /// <returns>Debug string<br/>调试字符串</returns>
        public override string ToString()
        {
            return m_Map.ToString();
        }

        /// <summary>
        /// Creates a variable set with the given byte order.
        /// 创建指定字节序的变量集合。
        /// </summary>
        /// <param name="littleEndian">Whether to use little-endian encoding<br/>是否使用小端编码</param>
        public VarSet(bool littleEndian)
        {
            m_Map = new CodingMap(littleEndian);
            m_KeySet = new HashSet<string>();
            m_TempKeySet = new HashSet<string>();
        }

        /// <inheritdoc/>
        public int Size => m_Map.Size;

        /// <inheritdoc/>
        public int KeySize => m_KeySet.Count;

        /// <inheritdoc/>
        public string KeyToStampKey(string key)
        {
            return key + ":";
        }

        /// <inheritdoc/>
        public void Clear()
        {
            m_KeySet.Clear();
            m_Map.Clear();
        }

        /// <inheritdoc/>
        public void SetVar(string key, object value)
        {
            m_KeySet.Add(key);
            if (value is V3Int v3)
            {
                m_Map.SetValue(key, new[] { v3.x, v3.y, v3.z });
                return;
            }

            if (value is V2Int v2)
            {
                m_Map.SetValue(key, new[] { v2.x, v2.y });
                return;
            }

            m_Map.SetValue(key, value);
        }

        /// <inheritdoc/>
        public void SetVar(string key, object value, long timestamp)
        {
            SetVar(key, value);
            m_Map.SetValue(KeyToStampKey(key), timestamp);
        }

        /// <inheritdoc/>
        public object DeleteVar(string key, bool includeStampKey)
        {
            m_KeySet.Remove(key);
            if (includeStampKey)
            {
                var stampKey = KeyToStampKey(key);
                m_Map.DeleteValue(stampKey);
            }

            return m_Map.DeleteValue(key);
        }

        /// <inheritdoc/>
        public void SetVars(Dictionary<string, object> vars)
        {
            if (null == vars || vars.Count == 0) return;
            foreach (var pair in vars)
            {
                SetVar(pair.Key, pair.Value);
            }
        }

        /// <inheritdoc/>
        public void SetVars(Dictionary<string, object> vars, long timestamp)
        {
            if (null == vars || vars.Count == 0) return;
            foreach (var pair in vars)
            {
                SetVar(pair.Key, pair.Value, timestamp);
            }
        }

        /// <inheritdoc/>
        public void SetVars(IVarSet set)
        {
            set?.ForEach((key, value) => SetVar(key, value));
        }

        /// <inheritdoc/>
        public void SetVars(IVarSet set, long timestamp)
        {
            set?.ForEach((key, value) => SetVar(key, value, timestamp));
        }

        /// <inheritdoc/>
        public void DeleteVars(string[] keys, bool includeStampKey)
        {
            if (null == keys || keys.Length == 0) return;
            for (var index = 0; index < keys.Length; index++)
            {
                DeleteVar(keys[index], includeStampKey);
            }
        }

        /// <inheritdoc/>
        public bool CheckKey(string key)
        {
            return m_Map.CheckKey(key);
        }

        /// <inheritdoc/>
        public object GetValue(string key)
        {
            return m_Map.GetValue(key);
        }

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            if (typeof(T) == typeof(V3Int))
            {
                var arr = m_Map.GetValue<int[]>(key);
                if (arr == null || arr.Length != 3)
                {
                    return default(T);
                }

                object rs = new V3Int(arr[0], arr[1], arr[2]);
                return (T)rs;
            }

            if (typeof(T) == typeof(V2Int))
            {
                var arr = m_Map.GetValue<int[]>(key);
                if (arr == null || arr.Length != 2)
                {
                    return default(T);
                }

                object rs = new V2Int(arr[0], arr[1]);
                return (T)rs;
            }

            return m_Map.GetValue<T>(key);
        }

        /// <inheritdoc/>
        public long GetValueStamp(string key)
        {
            var stampKey = KeyToStampKey(key);
            if (m_Map.CheckKey(stampKey))
            {
                return m_Map.GetValue<long>(stampKey);
            }

            return 0;
        }

        /// <inheritdoc/>
        public void ForEach(VarSetDelegates.FuncEach each)
        {
            if (null == each) return;
            m_Map.ForEach((key, value) => each(key, value));
        }

        /// <inheritdoc/>
        public void ForEach(VarSetDelegates.FuncStampEach stampEach)
        {
            if (null == stampEach) return;
            foreach (var key in m_KeySet)
            {
                stampEach.Invoke(key, m_Map.GetValue(key), GetValueStamp(key));
            }
        }

        /// <summary>
        /// Encodes all variables to a byte array.
        /// 将所有变量编码为字节数组。
        /// </summary>
        /// <returns>Encoded bytes<br/>编码后的字节</returns>
        public byte[] EncodeToBytes()
        {
            return m_Map.ToBinary();
        }

        /// <summary>
        /// Decodes variables from a byte array.
        /// 从字节数组解码变量。
        /// </summary>
        /// <param name="bytes">Encoded bytes<br/>编码字节</param>
        public void DecodeFromBytes(byte[] bytes)
        {
            m_Map.Clear();
            m_Map.FromBinaryOverride(bytes);
            RebuildKeySet();
        }

        /// <summary>
        /// Encodes variables into a data buffer writer.
        /// 将变量编码写入数据缓冲区写入器。
        /// </summary>
        /// <param name="buff">Buffer writer<br/>缓冲区写入器</param>
        public void EncodeToBuff(IDataBufferWriter buff)
        {
            var bs = m_Map.ToBinary();
            buff.WriteData(bs);
        }

        /// <summary>
        /// Decodes variables from a data buffer reader.
        /// 从数据缓冲区读取器解码变量。
        /// </summary>
        /// <param name="buff">Buffer reader<br/>缓冲区读取器</param>
        public void DecodeFromBuff(IDataBufferReader buff)
        {
            var bs = buff.ReadUInt8Array();
            DecodeFromBytes(bs);
        }

        private void RebuildKeySet()
        {
            m_TempKeySet.Clear();
            m_KeySet.Clear();
            m_Map.ForEach((key, value) =>
            {
                m_TempKeySet.Add(key);
                m_KeySet.Add(key);
            });
            foreach (var key in m_TempKeySet)
            {
                var stampKey = KeyToStampKey(key);
                if (m_KeySet.Contains(stampKey))
                {
                    m_KeySet.Remove(stampKey);
                }
            }
        }
    }
}
