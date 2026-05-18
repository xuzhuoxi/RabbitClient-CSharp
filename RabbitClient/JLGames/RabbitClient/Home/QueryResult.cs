namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Result of a route query to Rabbit-Home.
    /// 向 Rabbit-Home 查询路由的结果
    /// </summary>
    public struct QueryResult
    {
        /// <summary>
        /// Whether the query succeeded.
        /// 查询是否成功
        /// </summary>
        public bool Ok { get; internal set; }

        /// <summary>
        /// Whether public key loading or parsing failed.
        /// 公钥加载或解析是否失败
        /// </summary>
        public bool KeyError { get; internal set; }

        /// <summary>
        /// Whether request parameters are invalid.
        /// 请求参数是否无效
        /// </summary>
        public bool ParamError { get; internal set; }

        /// <summary>
        /// Whether the request timed out.
        /// 请求是否超时
        /// </summary>
        public bool TimeOut { get; internal set; }

        /// <summary>
        /// Route info returned on success.
        /// 查询成功时返回的路由信息
        /// </summary>
        public QueryRouteBackInfo SucInfo { get; internal set; }

        /// <summary>
        /// Error info returned on failure.
        /// 查询失败时返回的错误信息
        /// </summary>
        public HomeResponseInfo FailInfo { get; internal set; }

        /// <summary>
        /// Returns a string representation of the result.
        /// 返回结果的字符串表示
        /// </summary>
        /// <returns>String with status fields and success/failure info<br/> 包含各状态字段及成功/失败信息的字符串</returns>
        public override string ToString()
        {
            return
                $"QueryResult{{ok={Ok}, keyError={KeyError}, paramError={ParamError}, timeOut={TimeOut}, sucInfo={SucInfo}, failInfo={FailInfo}}}";
        }
    }
}
