# 命名空间：JLGames.RabbitClient.Home

与 Rabbit-Home 交互：配置、路由查询、密钥加载与默认常量。

## HomeSettings

Rabbit-Home 客户端连接与加密配置。

- 属性：
  - `HomeUrl`：服务地址。
  - `UsePost`：是否使用 POST。
  - `EnableKey`：是否启用 RSA 公钥加密。
  - `IsPemKey`：公钥是否为 PEM / X509（否则为 PKCS#1 v1.5）。
  - `PublicKeyPath`：公钥文件路径。
  - `PublicKeyContent`：公钥文本内容。
- 构造函数：
  - `HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey)`
- 方法：
  - `SetPublicKeyPath(string publicKeyPath)`
  - `SetPublicKeyContent(string publicKeyContent)`

加载公钥时优先使用 `PublicKeyContent`，为空再用 `PublicKeyPath`。

## HomeResponseInfo

Home 查询失败时的响应信息。JSON 字段：`code` / `value` / `other`。

- 属性：`ExtCode`、`Info`、`Other`
- 构造函数：`HomeResponseInfo(int extCode, string info, string other)`
- 方法：`FromJsonOverride(string json)`、`FromJsonString(string json)`、`ToString()`

## QueryResult

向 Home 查询路由的结果（结构体）。

- `Ok`：是否成功。
- `KeyError`：公钥加载或解析失败。
- `ParamError`：请求参数无效。
- `TimeOut`：请求超时。
- `SucInfo`：成功时的 `QueryRouteBackInfo`。
- `FailInfo`：失败时的 `HomeResponseInfo`。
- `ToString()`

## QueryRouteInfo

查找可用 Rabbit-Server 实例的请求。JSON 字段：`pid` / `type-name` / `temp-key`。

- `PlatformId`：服务平台 Id。
- `TypeName`：类型名称。
- `TempAesKey`：临时 AES 密钥（32 字节），用于 Home 返回会话密钥时加密；不提供时 `OpenBase64Sk` 仅作 Base64。
- `ToJsonString()`、`ToString()`

## QueryRouteBackInfo

Home 返回的可用实例信息。JSON 字段：`id` / `pid` / `type-name` / `open-network` / `open-addr` / `open-key-on` / `open-sk`。

- `Id`：实例唯一 Id。
- `PlatformId`：服务平台 Id。
- `TypeName`：实例类型名称。
- `OpenNetwork`：开放连接协议。
- `OpenAddr`：开放连接地址。
- `OpenKeyOn`：是否启用密钥。
- `OpenBase64Sk`：会话密钥的 Base64；请求带了临时密钥时该字段已加密。
- `OpenSk`：会话密钥字节，执行 `ComputeOpenSk` 后更新。
- `ComputeOpenSk(byte[] tempAesKey)`：用 32 字节临时 AES 解密 `OpenBase64Sk` 写入 `OpenSk`；临时密钥为空则只做 Base64 解码。
- `FromJsonOverride(string json)`、`FromJsonString(string json)`、`ToString()`

## RabbitHomeClient

向 Home 的 `/route` 查询可用实例。实现 `IDisposable`。默认 HTTP 超时 100 秒。查询参数键为 `q`。

- 属性：`HomeUrl`
- 构造函数：
  - `RabbitHomeClient(string homeUrl, bool usePost)`
  - `RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)`
  - `RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost)`
  - `RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost, TimeSpan timeout)`
- 方法：
  - `SetPublicRsa(IRsaPublicCipher pub)` / `SetPublicRsa(RSA pubKey)`
  - `QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath, string publicKeyContent)`：从路径或内容加载公钥；加载失败时 `KeyError = true`。
  - `QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)`
  - `QueryFromHome(QueryRouteInfo queryInfo)`：使用已设置的公钥；未设置则不加密请求体。`queryInfo` 为 null 时 `ParamError = true`。
  - `Dispose()`

请求体为 `QueryRouteInfo` 的 JSON，UTF-8 编码后可选 RSA 加密，再按 `RabbitHomeDefaults.Base64Encoding`（默认 `Base64RawUrlEncoding`）编码为 `q`。

## RabbitHomeDefaults

Home 通信默认配置。

- `HttpKeyQuery`：`"q"`
- `HttpPatternRoute`：`"/route"`
- `LittleEndian`：默认 `true`；`SetLittleEndian(bool)`
- `Base64Encoding`：默认 `Base64RawUrlEncoding`；`SetBase64Encoding(IBase64Encoding)`

## RabbitHomeUtils

加载 Home 通信用 RSA 公钥。PEM / X509 与 PKCS#1 v1.5 由 `isPemKey` 决定。内容非空时优先于路径。

- `LoadHomePublicRsa(HomeSettings homeSettings)`
- `LoadHomePublicRsa(bool isPemKey, string pubKeyPath, string pubKeyContent)`
- `LoadHomePublicRsaWithPath(bool isPemKey, string pubKeyPath)`
- `LoadHomePublicRsaWithContent(bool isPemKey, string pubKeyContent)`

路径与内容均为空时返回 `null`。
