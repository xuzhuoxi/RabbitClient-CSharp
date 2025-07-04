using JLGames.Infra.Net;

namespace JLGames.RabbitClient.Server
{
    public class RabbitRequestMsg : RabbitMessageWriter, IRabbitRequestMsg
    {
        private readonly bool m_LittleEndian;

        public RabbitRequestMsg(bool littleEndian = true) : base(littleEndian)
        {
            m_LittleEndian = littleEndian;
        }

        public void SetClientId(string cid)
        {
            m_Header.ClientId = cid;
        }

        public void SetProtoInfo(string extName, string protoId)
        {
            m_Header.Extension = extName;
            m_Header.ProtoId = protoId;
        }

        public void StartWriteData()
        {
            WriteHeader();
        }

        public void WriteRequestBase(object baseValue)
        {
            if (null == baseValue) return;
            WriteData(baseValue);
        }

        public void WriteRequestMessage(INetMessage reqMsg)
        {
            if (null == reqMsg) return;
            WriteMessage(reqMsg);
        }
    }
}