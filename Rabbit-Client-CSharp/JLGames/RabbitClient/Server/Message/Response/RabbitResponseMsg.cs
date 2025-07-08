namespace JLGames.RabbitClient.Server.Message
{
    public class RabbitResponseMsg : RabbitMessageReader, IRabbitResponseMsg
    {
        private int m_RsCode;
        private readonly bool m_LittleEndian;
        private byte[] m_OrignalBytes;

        public int RsCode => m_RsCode;

        public RabbitResponseMsg(bool littleEndian = true) : base(littleEndian)
        {
            m_LittleEndian = littleEndian;
        }

        public override void StartReadData()
        {
            base.StartReadData();
            m_RsCode = m_Unpacker.ReadInt32();
        }

        public new void SetMessageBytes(byte[] msg)
        {
            m_OrignalBytes = msg;
            m_Unpacker.WriteMessageBytes(msg);
        }

        public IRabbitResponseMsg Clone()
        {
            var rs = new RabbitResponseMsg(m_LittleEndian);
            rs.SetMessageBytes(m_OrignalBytes);
            rs.StartReadData();
            return rs;
        }
    }
}