using JLGames.Infra.Encodingx.Base64x;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Default settings for Rabbit-Home communication.
    /// Rabbit-Home 通信相关的默认配置项
    /// </summary>
    public static class RabbitHomeDefaults
    {
        /// <summary>
        /// HTTP query parameter key name.
        /// HTTP 查询参数键名
        /// </summary>
        public const string HttpKeyQuery = "q";

        /// <summary>
        /// HTTP path pattern for route query.
        /// 路由查询的 HTTP 路径模式
        /// </summary>
        public const string HttpPatternRoute = "/route";

        /// <summary>
        /// Whether byte order is little-endian.
        /// 字节序是否为小端
        /// </summary>
        public static bool LittleEndian { get; private set; } = true;

        /// <summary>
        /// Base64 encoder for encoding/decoding request and response content.
        /// 用于编解码请求/响应内容的 Base64 编码器
        /// </summary>
        public static IBase64Encoding Base64Encoding { get; private set; } = new Base64RawUrlEncoding();

        /// <summary>
        /// Sets the byte order.
        /// 设置字节序
        /// </summary>
        /// <param name="littleEndian">Use little-endian when true<br/> 为 true 时使用小端字节序</param>
        public static void SetLittleEndian(bool littleEndian)
        {
            LittleEndian = littleEndian;
        }

        /// <summary>
        /// Sets the Base64 encoder.
        /// 设置 Base64 编码器
        /// </summary>
        /// <param name="encoding">Custom Base64 encoding implementation<br/> 自定义 Base64 编码实现</param>
        public static void SetBase64Encoding(IBase64Encoding encoding)
        {
            Base64Encoding = encoding;
        }
    }
}
