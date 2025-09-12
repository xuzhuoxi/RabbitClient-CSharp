using System.Runtime.Serialization;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// 查找合适的Rabbit-Server的实例
    /// </summary>
    public class QueryRouteInfo
    {
        /// <summary>
        /// 服务平台Id
        /// </summary>
        [DataMember(Name = "pid")]
        public string PlatformId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [DataMember(Name = "type-name")]
        public string TypeName { get; set; }

        /// <summary>
        /// 临时AES密钥，用于Rabbit-Home返回数据时加密, 如果不提供，返回的密钥数据将以Base64字符串返回
        /// </summary>
        [DataMember(Name = "temp-key")]
        public byte[] TempAesKey { get; set; }

        public override string ToString()
        {
            return $"QueryRouteInfo{{PlatformId:{PlatformId}, TypeName:{TypeName}, TempAesKey:{TempAesKey}}}";
        }

        /// <summary>
        /// 序列化为Json字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToJsonString()
        {
            return this.ToJson();
            // .Replace("PlatformId", "pid")
            // .Replace("TypeName", "type-name")
            // .Replace("TempAesKey", "temp-key");
        }
    }
}