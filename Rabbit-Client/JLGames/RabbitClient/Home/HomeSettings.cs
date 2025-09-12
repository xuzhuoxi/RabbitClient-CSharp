namespace JLGames.RabbitClient.Home
{
    public class HomeSettings
    {
        public string HomeUrl { get; private set; }
        public bool UsePost { get; private set; }
        public bool EnableKey { get; private set; }
        public bool IsPemKey { get; private set; }
        public string PublicKeyPath { get; private set; }

        public HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey, string publicKeyPath)
        {
            HomeUrl = homeUrl;
            UsePost = usePost;

            EnableKey = enableKey;
            IsPemKey = isPemKey;
            PublicKeyPath = publicKeyPath;
        }
    }
}