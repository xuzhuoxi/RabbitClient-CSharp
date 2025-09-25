using JLGames.Infra.Encodingx.Base64x;

namespace JLGames.RabbitClient.Home
{
    public static class RabbitHomeDefaults
    {
        public const string HttpKeyQuery = "q";
        public const string HttpPatternRoute = "/route";
        public static bool LittleEndian { get; private set; } = true;

        public static IBase64Encoding Base64Encoding { get; private set; } = new Base64RawUrlEncoding();

        public static void SetLittleEndian(bool littleEndian)
        {
            LittleEndian = littleEndian;
        }

        public static void SetBase64Encoding(IBase64Encoding encoding)
        {
            Base64Encoding = encoding;
        }
    }
}
