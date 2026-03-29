using System;
using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server.Message
{
    public class RabbitMessageWriter : IRabbitMessageWriter
    {
        internal RabbitMessageHeader m_MessageHeader;
        protected readonly INetMessageWriter m_Packer;

        public string Extension => m_MessageHeader.Extension;
        public string ProtoId => m_MessageHeader.ProtoId;
        public string ClientId => m_MessageHeader.ClientId;
        public string ProtoUid => m_MessageHeader.ProtoUid;

        public RabbitMessageWriter(bool littleEndian = true)
        {
            m_MessageHeader = new RabbitMessageHeader();
            m_Packer = new NetMessageWriter(littleEndian);
        }

        public void WriteHeader()
        {
            m_Packer.Clear();
            m_Packer.WriteData(m_MessageHeader.Extension);
            m_Packer.WriteData(m_MessageHeader.ProtoId);
            m_Packer.WriteData(m_MessageHeader.ClientId);
        }

        public void WriteHeader(string extension, string protoId, string cid)
        {
            m_MessageHeader.SetHeaderInfo(extension, protoId, cid);
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
