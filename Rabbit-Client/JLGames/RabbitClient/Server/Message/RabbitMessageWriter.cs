using System;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    public class RabbitMessageWriter : IRabbitMessageWriter
    {
        internal RabbitHeader m_Header;
        protected readonly INetMessageWriter m_Packer;

        public string Extension => m_Header.Extension;
        public string ProtoId => m_Header.ProtoId;
        public string ClientId => m_Header.ClientId;
        public string ProtoUid => m_Header.ProtoUid;

        public RabbitMessageWriter(bool littleEndian = true)
        {
            m_Header = new RabbitHeader();
            m_Packer = new NetMessageWriter(littleEndian);
        }

        public void WriteHeader()
        {
            m_Packer.Clear();
            m_Packer.WriteData(m_Header.Extension);
            m_Packer.WriteData(m_Header.ProtoId);
            m_Packer.WriteData(m_Header.ClientId);
        }

        public void WriteHeader(string extension, string protoId, string cid)
        {
            m_Header.SetHeaderInfo(extension, protoId, cid);
            WriteHeader();
        }

        public void WriteMessage(INetMessage msg)
        {
            m_Packer.WriteData(msg.EncodeToBytes());
        }

        public void WriteData(object data)
        {
            if (data is INetMessage message)
            {
                m_Packer.WriteMessage(message);
                return;
            }

            try
            {
                m_Packer.WriteBaseData(data);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public byte[] ToMessageBytes()
        {
            return m_Packer.ReadMessageBytes();
        }
    }
}