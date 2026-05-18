using System;
using System.Runtime.Serialization;
using JLGames.Infra.Crypto.Symmetric;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// Route query result returned from Rabbit-Home for a suitable Rabbit-Server instance.
    /// 查找合适的 Rabbit-Server 实例的返回信息，从 Rabbit-Home 返回
    /// </summary>
    public class QueryRouteBackInfo
    {
        /// <summary>
        /// Instance id (unique).
        /// 实例 Id（唯一）
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// Instance service platform id.
        /// 实例服务平台 Id
        /// </summary>
        [DataMember(Name = "pid")]
        public string PlatformId { get; private set; }

        /// <summary>
        /// Instance type name (not unique).
        /// 实例类型名称（不唯一）
        /// </summary>
        [DataMember(Name = "type-name")]
        public string TypeName { get; private set; }

        /// <summary>
        /// Open connection network protocol.
        /// 开放连接通信协议
        /// </summary>
        [DataMember(Name = "open-network")]
        public string OpenNetwork { get; private set; }

        /// <summary>
        /// Open connection address.
        /// 开放连接地址
        /// </summary>
        [DataMember(Name = "open-addr")]
        public string OpenAddr { get; private set; }

        /// <summary>
        /// Whether key verification is enabled for the client.
        /// 针对客户端是否启用密钥验证
        /// </summary>
        [DataMember(Name = "open-key-on")]
        public bool OpenKeyOn { get; private set; }

        /// <summary>
        /// Base64 representation of the session key for symmetric encryption; encrypted if a temp key was sent in the request.
        /// 临时密钥的 Base64 字符串表示，用于对称加密数据；请求时有设置临时密钥则经过加密
        /// </summary>
        [DataMember(Name = "open-sk")]
        public string OpenBase64Sk { get; private set; }

        /// <summary>
        /// Session key bytes, updated after <see cref="ComputeOpenSk"/> for symmetric encryption.
        /// 临时密钥，执行 ComputeOpenSk 后更新，用于对称加密数据
        /// </summary>
        public byte[] OpenSk { get; private set; }

        /// <summary>
        /// Returns a string representation of the route result.
        /// 返回路由结果的字符串表示
        /// </summary>
        /// <returns>String with instance id, address, keys, etc.<br/> 包含实例 Id、连接地址及密钥等字段的字符串</returns>
        public override string ToString()
        {
            var openSk = null == OpenSk ? "[]" : $"[{string.Join(" ", OpenSk)}]";
            return
                $"QueryRouteBackInfo{{Id={Id}, PlatformId={PlatformId}, TypeName={TypeName}, OpenNetwork={OpenNetwork}, OpenAddr={OpenAddr}, OpenKeyOn={OpenKeyOn}, OpenBase64Sk={OpenBase64Sk}, OpenSk={openSk}}}";
        }

        /// <summary>
        /// Decrypts <see cref="OpenBase64Sk"/> with the temp AES key and writes the session key to <see cref="OpenSk"/>.
        /// 通过临时 AES 密钥对 OpenBase64Sk 进行解密，得到通信密钥并写入 OpenSk
        /// </summary>
        /// <param name="tempAesKey">32-byte temp AES key from the request; null/empty decodes Base64 only<br/> 请求时提供的 32 字节临时 AES 密钥；为 null 或空时直接解码 Base64</param>
        /// <returns>Whether decryption or decode succeeded<br/> 解密或解码是否成功</returns>
        public bool ComputeOpenSk(byte[] tempAesKey)
        {
            if (string.IsNullOrEmpty(OpenBase64Sk) || !OpenKeyOn)
            {
                return true;
            }

            try
            {
                var openSk = RabbitHomeDefaults.Base64Encoding.DecodeBytesFrom(OpenBase64Sk);

                if (tempAesKey == null || tempAesKey.Length == 0)
                {
                    OpenSk = openSk;
                    return true;
                }

                if (tempAesKey.Length == 32)
                {
                    var cipher = new AesCipher(tempAesKey);
                    OpenSk = cipher.Decrypt(openSk);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Parses JSON and overwrites fields on the current instance.
        /// 从 Json 中解析并覆盖当前实例的字段
        /// </summary>
        /// <param name="json">JSON string<br/> JSON 字符串</param>
        public void FromJsonOverride(string json)
        {
            var obj = FromJsonString(json);
            Id = obj.Id;
            PlatformId = obj.PlatformId;
            TypeName = obj.TypeName;
            OpenNetwork = obj.OpenNetwork;
            OpenAddr = obj.OpenAddr;
            OpenKeyOn = obj.OpenKeyOn;
            OpenBase64Sk = obj.OpenBase64Sk;
            OpenSk = obj.OpenSk;
        }

        /// <summary>
        /// Parses from JSON.
        /// 从 Json 中解析
        /// </summary>
        /// <param name="json">JSON string<br/> JSON 字符串</param>
        /// <returns>Parsed route back info instance<br/> 解析后的路由返回信息实例</returns>
        public static QueryRouteBackInfo FromJsonString(string json)
        {
            var rs = json.FromJson<QueryRouteBackInfo>();
            return rs;
        }
    }
}
