namespace JLGames.RabbitClient.Server.MMO
{
    public static class VarSetDelegates
    {
        public delegate void FuncEach(string key, object value);

        public delegate void FuncStampEach(string key, object value, long stamp);
    }
}