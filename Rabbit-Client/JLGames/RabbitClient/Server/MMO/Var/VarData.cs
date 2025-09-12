using System;

namespace JLGames.RabbitClient.Server.MMO
{
    public struct VarData<T>
    {
        private string m_Key;
        private VarType m_Type;
        private T m_Value;
        private long m_Stamp;

        public string Key
        {
            get => m_Key;
            set => m_Key = value;
        }

        public VarType Type
        {
            get => m_Type;
            set => m_Type = value;
        }

        public T Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                m_Stamp = DateTime.Now.Ticks;
            }
        }

        public long Stamp
        {
            get => m_Stamp;
            set => m_Stamp = value;
        }
    }
}