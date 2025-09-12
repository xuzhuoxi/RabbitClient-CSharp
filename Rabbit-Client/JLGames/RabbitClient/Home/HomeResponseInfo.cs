using System.Runtime.Serialization;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    public class HomeResponseInfo
    {
        /// <summary>
        /// 扩展状态码
        /// </summary>
        [DataMember(Name = "code")]
        public int ExtCode { get; private set; }

        /// <summary>
        /// 返回信息
        /// 通常为通过base64转化的json字符串
        /// </summary>
        [DataMember(Name = "value")]
        public string Info { get; private set; }

        /// <summary>
        /// 其它返回信息
        /// 通常为通过base64转化的json字符串
        /// </summary>
        [DataMember(Name = "other")]
        public string Other { get; private set; }

        public override string ToString()
        {
            return $"HomeResponseInfo{{ExtCode={ExtCode}, Info={Info}, Other={Other}}}";
        }

        /// <summary>
        /// 从Json中解析
        /// </summary>
        /// <param name="json"></param>
        public void FromJsonOverride(string json)
        {
            var obj = FromJsonString(json);
            ExtCode = obj.ExtCode;
            Info = obj.Info;
            Other = obj.Other;
        }

        /// <summary>
        /// 从Json中解析
        /// </summary>
        /// <param name="json"></param>
        public static HomeResponseInfo FromJsonString(string json)
        {
            return json.FromJson<HomeResponseInfo>();
        }
    }
}