namespace JLGames.RabbitClient.Home
{
    public class HomeSettings
    {
        public string HomeUrl { get; private set; }
        public bool UsePost { get; private set; }
        public bool EnableKey { get; private set; }
        public bool IsPemKey { get; private set; }
        public string PublicKeyPath { get; private set; }
        public string PublicKeyContent { get; private set; }

        public HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey)
        {
            HomeUrl = homeUrl;
            UsePost = usePost;
            EnableKey = enableKey;
            IsPemKey = isPemKey;
        }

        public void SetPublicKeyPath(string publicKeyPath)
        {
            PublicKeyPath = publicKeyPath;
        }

        public void SetPublicKeyContent(string publicKeyContent)
        {
            PublicKeyContent = publicKeyContent;
        }
    }
}
