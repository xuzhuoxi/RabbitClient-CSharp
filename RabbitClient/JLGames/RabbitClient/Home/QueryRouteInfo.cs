using System.Runtime.Serialization;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Request to find a suitable Rabbit-Server instance.
    /// 查找合适的 Rabbit-Server 实例
    /// </summary>
    public class QueryRouteInfo
    {
        /// <summary>
        /// Service platform id.
        /// 服务平台 Id
        /// </summary>
        [DataMember(Name = "pid")]
        public string PlatformId { get; set; }

        /// <summary>
        /// Type name.
        /// 类型名称
        /// </summary>
        [DataMember(Name = "type-name")]
        public string TypeName { get; set; }

        /// <summary>
        /// Temporary AES key for encrypting Rabbit-Home response data; if omitted, returned key data is base64 only.
        /// 临时 AES 密钥，用于 Rabbit-Home 返回数据时加密；不提供时返回的密钥数据将以 Base64 字符串返回
        /// </summary>
        [DataMember(Name = "temp-key")]
        public byte[] TempAesKey { get; set; }

        /// <summary>
        /// Returns a string representation of the query parameters.
        /// 返回查询参数的字符串表示
        /// </summary>
        /// <returns>String with platform id, type name and temp key<br/> 包含平台 Id、类型名及临时密钥信息的字符串</returns>
        public override string ToString()
        {
            return $"QueryRouteInfo{{PlatformId:{PlatformId}, TypeName:{TypeName}, TempAesKey:{TempAesKey}}}";
        }

        /// <summary>
        /// Serializes to a JSON string.
        /// 序列化为 Json 字符串
        /// </summary>
        /// <returns>JSON query parameter string<br/> JSON 格式的查询参数字符串</returns>
        public virtual string ToJsonString()
        {
            return this.ToJson();
            // .Replace("PlatformId", "pid")
            // .Replace("TypeName", "type-name")
            // .Replace("TempAesKey", "temp-key");
        }
    }
}
