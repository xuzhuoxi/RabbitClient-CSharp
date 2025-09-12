namespace JLGames.RabbitClient.Server.Message
{
    internal struct RabbitHeader
    {
        public string Extension;
        public string ProtoId;
        public string ClientId;

        public string ProtoUid => $"{Extension}:{ProtoId}";

        public void SetHeaderInfo(string extension, string protoId, string cid)
        {
            Extension = extension;
            ProtoId = protoId;
            ClientId = cid;
        }
    }
}
