namespace JLGames.RabbitClient.Server
{
    public static class RabbitServerDefaults
    {
        public static bool LittleEndian { get; private set; } = true;

        public static bool ApmMode { get; private set; }

        public static void SetLittleEndian(bool littleEndian)
        {
            LittleEndian = littleEndian;
        }

        public static void SetConnectApiMode(bool apmMode)
        {
            ApmMode = apmMode;
        }
    }
}
