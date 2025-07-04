namespace JLGames.RabbitClient.Server
{
    internal struct RabbitHeader
    {
        public string Extension;
        public string ProtoId;
        public string ClientId;

        public string ProtoUUID => $"{Extension}_{ProtoId}";

        public void SetHeaderInfo(string extension, string protoId, string cid)
        {
            Extension = extension;
            ProtoId = protoId;
            ClientId = cid;
        }
    }
}