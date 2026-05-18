namespace JLGames.RabbitClient.Server
{
    /// <summary>
    /// Default configuration values for Rabbit server socket and message encoding.
    /// Rabbit 服务端 Socket 与消息编码的默认配置。
    /// </summary>
    public static class RabbitServerDefaults
    {
        /// <summary>
        /// Whether little-endian byte order is used for network messages.
        /// 网络消息是否使用小端字节序。
        /// </summary>
        public static bool LittleEndian { get; private set; } = true;

        /// <summary>
        /// Whether the socket client uses APM (async) connection mode.
        /// Socket 客户端是否使用 APM（异步）连接模式。
        /// </summary>
        public static bool ApmMode { get; private set; }

        /// <summary>
        /// Sets the endianness used for network message encoding.
        /// 设置网络消息编码的字节序。
        /// </summary>
        /// <param name="littleEndian">True for little-endian, false for big-endian<br/> true 为小端，false 为大端</param>
        public static void SetLittleEndian(bool littleEndian)
        {
            LittleEndian = littleEndian;
        }

        /// <summary>
        /// Sets the socket client connection API mode.
        /// 设置 Socket 客户端连接 API 模式。
        /// </summary>
        /// <param name="apmMode">True to use APM async mode<br/> true 表示使用 APM 异步模式</param>
        public static void SetConnectApiMode(bool apmMode)
        {
            ApmMode = apmMode;
        }
    }
}
