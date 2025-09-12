namespace JLGames.RabbitClient.Home
{
    public struct QueryResult
    {
        public bool Ok { get; internal set; }
        public bool KeyError { get; internal set; }
        public bool ParamError { get; internal set; }
        public bool TimeOut { get; internal set; }
        public QueryRouteBackInfo SucInfo { get; internal set; }
        public HomeResponseInfo FailInfo { get; internal set; }

        public override string ToString()
        {
            return
                $"QueryResult{{ok={Ok}, keyError={KeyError}, paramError={ParamError}, timeOut={TimeOut}, sucInfo={SucInfo}, failInfo={FailInfo}}}";
        }
    }
}