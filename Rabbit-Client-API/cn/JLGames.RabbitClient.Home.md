# 命名空间：JLGames.RabbitClient.Home

本命名空间包含与 Rabbit-Home 服务器交互的相关 API。

## 类与结构体

### HomeResponseInfo
- 描述：Rabbit-Home 返回的响应信息。
- 主要属性：
  - `ExtCode`：扩展状态码。
  - `Info`：返回信息，通常为 base64 编码的 json 字符串。
  - `Other`：其它返回信息，通常为 base64 编码的 json 字符串。
- 主要方法：
  - `FromJsonOverride(string json)`：从 json 字符串解析。
  - `FromJsonString(string json)`：静态方法，从 json 字符串解析。

### HomeSettings
- 描述：Rabbit-Home 服务器的配置信息。
- 主要属性：
  - `HomeUrl`：Rabbit-Home 服务器地址。
  - `UsePost`：是否使用 POST 请求。
  - `EnableKey`：是否启用密钥。
  - `IsPemKey`：密钥是否为 PEM 格式。
  - `PublicKeyPath`：公钥路径。
- 构造函数：
  - `HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey, string publicKeyPath)`：初始化配置信息。

### QueryResult
- 描述：查询 Rabbit-Home 结果。
- 主要属性：
  - `Ok`：是否成功。
  - `KeyError`：密钥错误。
  - `ParamError`：参数错误。
  - `TimeOut`：超时。
  - `SucInfo`：成功时返回的服务器信息（`QueryRouteBackInfo`）。
  - `FailInfo`：失败时返回的信息（`HomeResponseInfo`）。
- 主要方法：
  - `ToString()`：返回结构体内容字符串。

### QueryRouteInfo
- 描述：用于向 Rabbit-Home 查询可用 Rabbit-Server 实例。
- 主要属性：
  - `PlatformId`：服务平台 Id。
  - `TypeName`：类型名称。
  - `TempAesKey`：临时 AES 密钥，用于 Rabbit-Home 返回数据时加密。如果不提供，返回的密钥数据将以 Base64 字符串返回。
- 主要方法：
  - `ToJsonString()`：序列化为 json 字符串。
  - `ToString()`：返回内容字符串。

### QueryRouteBackInfo
- 描述：Rabbit-Home 返回的可用 Rabbit-Server 实例信息。
- 主要属性：
  - `Id`：实例唯一 Id。
  - `PlatformId`：服务平台 Id。
  - `TypeName`：实例类型名称。
  - `OpenNetwork`：开放连接协议。
  - `OpenAddr`：开放连接地址。
  - `OpenKeyOn`：是否启用密钥验证。
  - `OpenBase64Sk`：临时密钥的 Base64 字符串。
  - `OpenSk`：临时密钥字节数组，执行 ComputeOpenSk 后更新。
- 主要方法：
  - `ComputeOpenSk(byte[] tempAesKey)`：通过临时 AES 密钥解密通信密钥。
  - `FromJsonOverride(string json)`：从 json 字符串解析。
  - `FromJsonString(string json)`：静态方法，从 json 字符串解析。
  - `ToString()`：返回内容字符串。

### RabbitHomeClient
- 描述：Rabbit-Home 客户端，负责与 Rabbit-Home 服务器通信，查询可用 Rabbit-Server 实例。
- 构造函数：
  - `RabbitHomeClient(string homeUrl, bool usePost)`：初始化客户端。
  - `RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)`：初始化客户端并设置超时时间。
- 主要方法：
  - `SetPublicRsa(IRsaPublicCipher pub)`：设置 RSA 公钥。
  - `SetPublicRsa(RSA pubKey)`：设置 RSA 公钥。
  - `QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath)`：向 Rabbit-Home 查询可用实例（支持 PEM/PKCS1 公钥文件）。
  - `QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)`：向 Rabbit-Home 查询可用实例（自定义公钥实现）。
  - `QueryFromHome(QueryRouteInfo queryInfo)`：向 Rabbit-Home 查询可用实例。

### RabbitHomeDefaults
- 描述：Rabbit-Home 相关常量和默认配置。
- 主要属性/方法：
  - `HttpKeyQuery`：查询参数名。
  - `HttpPatternRoute`：路由路径。
  - `LittleEndian`：字节序设置。
  - `Base64Encoding`：Base64 编码实现。
  - `SetLittleEndian(bool)`：设置字节序。
  - `SetBase64Encoding(IBase64Encoding)`：设置 Base64 编码实现。 