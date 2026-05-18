namespace JLGames.RabbitClient.Server.Message
{
    /// <summary>
    /// Rabbit server response message reader implementation.
    /// Rabbit服务端响应消息读取实现。
    /// </summary>
    public class RabbitResponseMsg : RabbitMessageReader, IRabbitResponseMsg
    {
        private int m_RsCode;
        private readonly bool m_LittleEndian;
        private byte[] m_OrignalBytes;

        /// <summary>
        /// Response result status code read from the message.
        /// 从消息中读取的响应状态码。
        /// </summary>
        public int RsCode => m_RsCode;

        /// <summary>
        /// Creates a response message reader with optional little-endian byte order.
        /// 创建响应消息读取器，可选小端字节序。
        /// </summary>
        /// <param name="littleEndian">Whether to use little-endian decoding<br/>是否使用小端解码</param>
        public RabbitResponseMsg(bool littleEndian = true) : base(littleEndian)
        {
            m_LittleEndian = littleEndian;
        }

        /// <summary>
        /// Reads the header and response result code from the buffer.
        /// 从缓冲区读取消息头与响应状态码。
        /// </summary>
        public override void StartReadData()
        {
            base.StartReadData();
            m_RsCode = m_Unpacker.ReadInt32();
        }

        /// <summary>
        /// Loads raw message bytes into the internal unpacker.
        /// 将原始消息字节加载到内部解包器。
        /// </summary>
        /// <param name="msg">Raw message bytes<br/>原始消息字节</param>
        public new void SetMessageBytes(byte[] msg)
        {
            m_OrignalBytes = msg;
            m_Unpacker.WriteMessageBytes(msg);
        }

        /// <summary>
        /// Creates a copy positioned at the start of the same message data.
        /// 创建一份定位到相同消息数据起始位置的副本。
        /// </summary>
        /// <returns>Cloned response reader<br/>克隆的响应读取器</returns>
        public IRabbitResponseMsg Clone()
        {
            var rs = new RabbitResponseMsg(m_LittleEndian);
            rs.SetMessageBytes(m_OrignalBytes);
            rs.StartReadData();
            return rs;
        }
    }
}
