using System;
using System.Runtime.Serialization;
using JLGames.Infra.Crypto.Symmetric;
using JLGames.Infra.TinyJson;

namespace JLGames.RabbitClient.Home
{
    /// <summary>
    /// 查找结果
    /// 查找合适的Rabbit-Server的实例返回信息，从Rabbit-Home返回
    /// </summary>
    public class QueryRouteBackInfo
    {
        /// <summary>
        /// 实例Id(唯一)
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// 实例服务平台Id
        /// </summary>
        [DataMember(Name = "pid")]
        public string PlatformId { get; private set; }

        /// <summary>
        /// 实例类型名称(不唯一)
        /// </summary>
        [DataMember(Name = "type-name")]
        public string TypeName { get; private set; }

        /// <summary>
        /// 开放连接通信协议
        /// </summary>
        [DataMember(Name = "open-network")]
        public string OpenNetwork { get; private set; }

        /// <summary>
        /// 开放连接地址
        /// </summary>
        [DataMember(Name = "open-addr")]
        public string OpenAddr { get; private set; }

        /// <summary>
        /// 针对客户端是否启用密钥验证
        /// </summary>
        [DataMember(Name = "open-key-on")]
        public bool OpenKeyOn { get; private set; }

        /// <summary>
        /// 临时密钥的Base64字符串表示，用于对称加密数据。如果请求时有设置临时密钥，则经过加密
        /// </summary>
        [DataMember(Name = "open-sk")]
        public string OpenBase64Sk { get; private set; }

        /// <summary>
        /// 临时密钥，执行ComputeOpenSK后更新，用于对称加密数据
        /// </summary>
        public byte[] OpenSk { get; private set; }

        public override string ToString()
        {
            var openSk = null == OpenSk ? "[]" : $"[{string.Join(" ", OpenSk)}]";
            return
                $"QueryRouteBackInfo{{Id={Id}, PlatformId={PlatformId}, TypeName={TypeName}, OpenNetwork={OpenNetwork}, OpenAddr={OpenAddr}, OpenKeyOn={OpenKeyOn}, OpenBase64Sk={OpenBase64Sk}, OpenSk={openSk}}}";
        }

        /// <summary>
        /// 通过临时Aes密钥，对OpenBase64SK进行解密，得到通信密钥
        /// </summary>
        /// <param name="tempAesKey"></param>
        /// <returns></returns>
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
            catch (Exception _)
            {
                return false;
            }
        }

        /// <summary>
        /// 从Json中解析
        /// </summary>
        /// <param name="json"></param>
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
        /// 从Json中解析
        /// </summary>
        /// <param name="json"></param>
        public static QueryRouteBackInfo FromJsonString(string json)
        {
            var rs = json.FromJson<QueryRouteBackInfo>();
            return rs;
        }
    }
}