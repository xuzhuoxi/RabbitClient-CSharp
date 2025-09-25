using JLGames.Infra.Crypto.Asymmetric;

namespace JLGames.RabbitClient.Home
{
    public static class RabbitHomeUtils
    {
        /// <summary>
        /// 加载用于 RabbitHome 通信的公钥
        /// </summary>
        /// <param name="homeSettings"></param>
        /// <returns></returns>
        public static IRsaPublicCipher LoadHomePublicRsa(HomeSettings homeSettings)
        {
            return LoadHomePublicRsa(homeSettings.IsPemKey, homeSettings.PublicKeyPath, homeSettings.PublicKeyContent);
        }

        /// <summary>
        /// 加载用于 RabbitHome 通信的公钥
        /// </summary>
        /// <param name="isPemKey"></param>
        /// <param name="pubKeyPath"></param>
        /// <param name="pubKeyContent"></param>
        /// <returns></returns>
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
        /// 加载用于 RabbitHome 通信的公钥
        /// </summary>
        /// <param name="isPemKey"></param>
        /// <param name="pubKeyPath"></param>
        /// <returns></returns>
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
        /// 加载用于 RabbitHome 通信的公钥
        /// </summary>
        /// <param name="isPemKey"></param>
        /// <param name="pubKeyContent"></param>
        /// <returns></returns>
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
