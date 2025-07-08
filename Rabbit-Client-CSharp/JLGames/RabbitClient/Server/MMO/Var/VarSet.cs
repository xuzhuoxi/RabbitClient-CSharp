using System.Collections.Generic;
using JLGames.Infra.Buffer;

namespace JLGames.RabbitClient.Server.MMO
{
    public class VarSet : IVarSet
    {
        private readonly CodingMap m_Map;
        private readonly HashSet<string> m_KeySet;
        private readonly HashSet<string> m_TempKeySet;

        public override string ToString()
        {
            return m_Map.ToString();
        }

        public VarSet(bool littleEndian)
        {
            m_Map = new CodingMap(littleEndian);
            m_KeySet = new HashSet<string>();
            m_TempKeySet = new HashSet<string>();
        }

        public int Size => m_Map.Size;

        public int KeySize => m_KeySet.Count;

        public string KeyToStampKey(string key)
        {
            return key + ":";
        }

        public void Clear()
        {
            m_KeySet.Clear();
            m_Map.Clear();
        }

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

        public void SetVar(string key, object value, long timestamp)
        {
            SetVar(key, value);
            m_Map.SetValue(KeyToStampKey(key), timestamp);
        }

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

        public void SetVars(Dictionary<string, object> vars)
        {
            if (null == vars || vars.Count == 0) return;
            foreach (var pair in vars)
            {
                SetVar(pair.Key, pair.Value);
            }
        }

        public void SetVars(Dictionary<string, object> vars, long timestamp)
        {
            if (null == vars || vars.Count == 0) return;
            foreach (var pair in vars)
            {
                SetVar(pair.Key, pair.Value, timestamp);
            }
        }

        public void SetVars(IVarSet set)
        {
            set?.ForEach((key, value) => SetVar(key, value));
        }

        public void SetVars(IVarSet set, long timestamp)
        {
            set?.ForEach((key, value) => SetVar(key, value, timestamp));
        }

        public void DeleteVars(string[] keys, bool includeStampKey)
        {
            if (null == keys || keys.Length == 0) return;
            for (var index = 0; index < keys.Length; index++)
            {
                DeleteVar(keys[index], includeStampKey);
            }
        }

        public bool CheckKey(string key)
        {
            return m_Map.CheckKey(key);
        }

        public object GetValue(string key)
        {
            return m_Map.GetValue(key);
        }

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

        public long GetValueStamp(string key)
        {
            var stampKey = KeyToStampKey(key);
            if (m_Map.CheckKey(stampKey))
            {
                return m_Map.GetValue<long>(stampKey);
            }

            return 0;
        }

        public void ForEach(VarSetDelegates.FuncEach each)
        {
            if (null == each) return;
            m_Map.ForEach((key, value) => each(key, value));
        }

        public void ForEach(VarSetDelegates.FuncStampEach stampEach)
        {
            if (null == stampEach) return;
            foreach (var key in m_KeySet)
            {
                stampEach.Invoke(key, m_Map.GetValue(key), GetValueStamp(key));
            }
        }

        public byte[] EncodeToBytes()
        {
            return m_Map.ToBinary();
        }

        public void DecodeFromBytes(byte[] bytes)
        {
            m_Map.Clear();
            m_Map.FromBinaryOverride(bytes);
            RebuildKeySet();
        }

        public void EncodeToBuff(IDataBufferWriter buff)
        {
            var bs = m_Map.ToBinary();
            buff.WriteData(bs);
        }

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