using System.Runtime.Serialization;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Response info when a Rabbit-Home query fails.
    /// Rabbit-Home 查询失败时的响应信息
    /// </summary>
    public class HomeResponseInfo
    {
        /// <summary>
        /// Extended status code.
        /// 扩展状态码
        /// </summary>
        [DataMember(Name = "code")]
        public int ExtCode { get; private set; }

        /// <summary>
        /// Primary response message, usually a base64-encoded JSON string.
        /// 返回信息，通常为通过 base64 转化的 json 字符串
        /// </summary>
        [DataMember(Name = "value")]
        public string Info { get; private set; }

        /// <summary>
        /// Additional response message, usually a base64-encoded JSON string.
        /// 其它返回信息，通常为通过 base64 转化的 json 字符串
        /// </summary>
        [DataMember(Name = "other")]
        public string Other { get; private set; }

        /// <summary>
        /// Returns a string representation of the response.
        /// 返回响应信息的字符串表示
        /// </summary>
        /// <returns>String with status code and info fields<br/> 包含状态码及信息字段的字符串</returns>
        public override string ToString()
        {
            return $"HomeResponseInfo{{ExtCode={ExtCode}, Info={Info}, Other={Other}}}";
        }

        /// <summary>
        /// Creates a failure response info.
        /// 创建失败响应信息
        /// </summary>
        /// <param name="extCode">Extended status code<br/> 扩展状态码</param>
        /// <param name="info">Primary response message<br/> 主要返回信息</param>
        /// <param name="other">Additional response message<br/> 其它返回信息</param>
        public HomeResponseInfo(int extCode, string info, string other)
        {
            ExtCode = extCode;
            Info = info;
            Other = other;
        }

        /// <summary>
        /// Parses JSON and overwrites fields on the current instance.
        /// 从 Json 中解析并覆盖当前实例的字段
        /// </summary>
        /// <param name="json">JSON string<br/> JSON 字符串</param>
        public void FromJsonOverride(string json)
        {
            var obj = FromJsonString(json);
            ExtCode = obj.ExtCode;
            Info = obj.Info;
            Other = obj.Other;
        }

        /// <summary>
        /// Parses from JSON.
        /// 从 Json 中解析
        /// </summary>
        /// <param name="json">JSON string<br/> JSON 字符串</param>
        /// <returns>Parsed response info instance<br/> 解析后的响应信息实例</returns>
        public static HomeResponseInfo FromJsonString(string json)
        {
            return json.FromJson<HomeResponseInfo>();
        }
    }
}
