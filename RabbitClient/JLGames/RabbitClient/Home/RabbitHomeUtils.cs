using JLGames.Infra.Crypto.Asymmetric;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Utility methods for Rabbit-Home.
    /// Rabbit-Home 相关工具方法
    /// </summary>
    public static class RabbitHomeUtils
    {
        /// <summary>
        /// Loads the RSA public key for Rabbit-Home communication from settings.
        /// 根据配置加载用于 Rabbit-Home 通信的 RSA 公钥
        /// </summary>
        /// <param name="homeSettings">Home settings; prefers key content over path<br/> Home 配置，优先使用公钥内容，其次使用公钥路径</param>
        /// <returns>Public key cipher, or null if path and content are empty<br/> 公钥加密器，路径与内容均为空时返回 null</returns>
        public static IRsaPublicCipher LoadHomePublicRsa(HomeSettings homeSettings)
        {
            return LoadHomePublicRsa(homeSettings.IsPemKey, homeSettings.PublicKeyPath, homeSettings.PublicKeyContent);
        }

        /// <summary>
        /// Loads the RSA public key for Rabbit-Home communication.
        /// 加载用于 Rabbit-Home 通信的 RSA 公钥
        /// </summary>
        /// <param name="isPemKey">PEM (X509) format when true, else PKCS#1 v1.5<br/> 公钥是否为 PEM（X509）格式，否则为 PKCS#1 v1.5</param>
        /// <param name="pubKeyPath">Public key file path<br/> 公钥文件路径</param>
        /// <param name="pubKeyContent">Public key text; used when non-empty<br/> 公钥文本内容，非空时优先于路径</param>
        /// <returns>Public key cipher, or null if path and content are empty<br/> 公钥加密器，路径与内容均为空时返回 null</returns>
        public static IRsaPublicCipher LoadHomePublicRsa(bool isPemKey, string pubKeyPath, string pubKeyContent)
        {
            if (!string.IsNullOrEmpty(pubKeyContent))
            {
                return LoadHomePublicRsaWithContent(isPemKey, pubKeyContent);
            }

            if (!string.IsNullOrEmpty(pubKeyPath))
            {
                return LoadHomePublicRsaWithPath(isPemKey, pubKeyPath);
            }

            return null;
        }

        /// <summary>
        /// Loads the RSA public key from a file path for Rabbit-Home communication.
        /// 从文件路径加载用于 Rabbit-Home 通信的 RSA 公钥
        /// </summary>
        /// <param name="isPemKey">PEM (X509) format when true, else PKCS#1 v1.5<br/> 公钥是否为 PEM（X509）格式，否则为 PKCS#1 v1.5</param>
        /// <param name="pubKeyPath">Public key file path<br/> 公钥文件路径</param>
        /// <returns>Public key cipher<br/> 公钥加密器</returns>
        public static IRsaPublicCipher LoadHomePublicRsaWithPath(bool isPemKey, string pubKeyPath)
        {
            IRsaPublicCipher pubCipher;
            if (isPemKey)
                pubCipher = RsaUtils.LoadPublicCipherX509(pubKeyPath);
            else
                pubCipher = RsaUtils.LoadPublicCipherPkcs1V15(pubKeyPath);
            return pubCipher;
        }

        /// <summary>
        /// Loads the RSA public key from text content for Rabbit-Home communication.
        /// 从文本内容加载用于 Rabbit-Home 通信的 RSA 公钥
        /// </summary>
        /// <param name="isPemKey">PEM (X509) format when true, else PKCS#1 v1.5<br/> 公钥是否为 PEM（X509）格式，否则为 PKCS#1 v1.5</param>
        /// <param name="pubKeyContent">Public key text content<br/> 公钥文本内容</param>
        /// <returns>Public key cipher<br/> 公钥加密器</returns>
        public static IRsaPublicCipher LoadHomePublicRsaWithContent(bool isPemKey, string pubKeyContent)
        {
            IRsaPublicCipher pubCipher;
            if (isPemKey)
                pubCipher = RsaUtils.LoadPublicCipherX509Content(pubKeyContent);
            else
                pubCipher = RsaUtils.LoadPublicCipherPkcs1V15Content(pubKeyContent);
            return pubCipher;
        }
    }
}
