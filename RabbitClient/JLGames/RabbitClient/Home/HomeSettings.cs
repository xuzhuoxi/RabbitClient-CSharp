namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Connection and encryption settings for the Rabbit-Home client.
    /// Rabbit-Home 客户端连接与加密相关配置
    /// </summary>
    public class HomeSettings
    {
        /// <summary>
        /// Rabbit-Home service URL.
        /// Rabbit-Home 服务地址
        /// </summary>
        public string HomeUrl { get; private set; }

        /// <summary>
        /// Whether to use POST for query requests.
        /// 是否使用 POST 方式发起查询请求
        /// </summary>
        public bool UsePost { get; private set; }

        /// <summary>
        /// Whether RSA public-key encryption is enabled.
        /// 是否启用 RSA 公钥加密
        /// </summary>
        public bool EnableKey { get; private set; }

        /// <summary>
        /// Whether the public key is in PEM format (otherwise PKCS#1 v1.5).
        /// 公钥是否为 PEM 格式（否则为 PKCS#1 v1.5）
        /// </summary>
        public bool IsPemKey { get; private set; }

        /// <summary>
        /// RSA public key file path.
        /// RSA 公钥文件路径
        /// </summary>
        public string PublicKeyPath { get; private set; }

        /// <summary>
        /// RSA public key text content.
        /// RSA 公钥文本内容
        /// </summary>
        public string PublicKeyContent { get; private set; }

        /// <summary>
        /// Creates Home settings.
        /// 创建 Home 配置
        /// </summary>
        /// <param name="homeUrl">Rabbit-Home service URL<br/> Rabbit-Home 服务地址</param>
        /// <param name="usePost">Use POST for query when true<br/> 是否使用 POST 方式查询</param>
        /// <param name="enableKey">Enable RSA public-key encryption<br/> 是否启用 RSA 公钥加密</param>
        /// <param name="isPemKey">Public key in PEM format when true<br/> 公钥是否为 PEM 格式</param>
        public HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey)
        {
            HomeUrl = homeUrl;
            UsePost = usePost;
            EnableKey = enableKey;
            IsPemKey = isPemKey;
        }

        /// <summary>
        /// Sets the RSA public key file path.
        /// 设置 RSA 公钥文件路径
        /// </summary>
        /// <param name="publicKeyPath">Public key file path<br/> 公钥文件路径</param>
        public void SetPublicKeyPath(string publicKeyPath)
        {
            PublicKeyPath = publicKeyPath;
        }

        /// <summary>
        /// Sets the RSA public key text content.
        /// 设置 RSA 公钥文本内容
        /// </summary>
        /// <param name="publicKeyContent">Public key text content<br/> 公钥文本内容</param>
        public void SetPublicKeyContent(string publicKeyContent)
        {
            PublicKeyContent = publicKeyContent;
        }
    }
}
