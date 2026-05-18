using System;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Single typed variable entry with optional timestamp.
    /// 带可选时间戳的单个类型化变量项。
    /// </summary>
    /// <typeparam name="T">Value type<br/>值类型</typeparam>
    public struct VarData<T>
    {
        private string m_Key;
        private VarType m_Type;
        private T m_Value;
        private long m_Stamp;

        /// <summary>
        /// Variable key name.
        /// 变量键名
        /// </summary>
        public string Key
        {
            get => m_Key;
            set => m_Key = value;
        }

        /// <summary>
        /// Variable persistence type.
        /// 变量持久类型
        /// </summary>
        public VarType Type
        {
            get => m_Type;
            set => m_Type = value;
        }

        /// <summary>
        /// Variable value; assigning updates the timestamp.
        /// 变量值；赋值时更新时间戳
        /// </summary>
        public T Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                m_Stamp = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Last update timestamp in ticks.
        /// 最后更新时间戳（Ticks）
        /// </summary>
        public long Stamp
        {
            get => m_Stamp;
            set => m_Stamp = value;
        }
    }
}
