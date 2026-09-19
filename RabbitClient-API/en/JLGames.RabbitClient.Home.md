# Namespace: JLGames.RabbitClient.Home

APIs for Rabbit-Home: settings, route queries, key loading, and defaults.

## HomeSettings

Connection and encryption settings for the Rabbit-Home client.

- Properties:
  - `HomeUrl`: Service URL.
  - `UsePost`: Whether to use POST.
  - `EnableKey`: Whether RSA public-key encryption is enabled.
  - `IsPemKey`: Whether the public key is PEM / X509 (otherwise PKCS#1 v1.5).
  - `PublicKeyPath`: Public key file path.
  - `PublicKeyContent`: Public key text content.
- Constructor:
  - `HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey)`
- Methods:
  - `SetPublicKeyPath(string publicKeyPath)`
  - `SetPublicKeyContent(string publicKeyContent)`

When loading a public key, `PublicKeyContent` is preferred over `PublicKeyPath`.

## HomeResponseInfo

Response info when a Home query fails. JSON fields: `code` / `value` / `other`.

- Properties: `ExtCode`, `Info`, `Other`
- Constructor: `HomeResponseInfo(int extCode, string info, string other)`
- Methods: `FromJsonOverride(string json)`, `FromJsonString(string json)`, `ToString()`

## QueryResult

Result of a route query to Home (struct).

- `Ok`: Whether the query succeeded.
- `KeyError`: Public key load or parse failed.
- `ParamError`: Request parameters are invalid.
- `TimeOut`: Request timed out.
- `SucInfo`: `QueryRouteBackInfo` on success.
- `FailInfo`: `HomeResponseInfo` on failure.
- `ToString()`

## QueryRouteInfo

Request to find a suitable Rabbit-Server instance. JSON fields: `pid` / `type-name` / `temp-key`.

- `PlatformId`: Service platform id.
- `TypeName`: Type name.
- `TempAesKey`: Temporary AES key (32 bytes) used to encrypt the Home response session key. If omitted, `OpenBase64Sk` is Base64 only.
- `ToJsonString()`, `ToString()`

## QueryRouteBackInfo

Available instance info returned by Home. JSON fields: `id` / `pid` / `type-name` / `open-network` / `open-addr` / `open-key-on` / `open-sk`.

- `Id`: Unique instance id.
- `PlatformId`: Service platform id.
- `TypeName`: Instance type name.
- `OpenNetwork`: Open connection protocol.
- `OpenAddr`: Open connection address.
- `OpenKeyOn`: Whether key verification is enabled.
- `OpenBase64Sk`: Base64 session key; encrypted when a temp key was sent.
- `OpenSk`: Session key bytes, updated after `ComputeOpenSk`.
- `ComputeOpenSk(byte[] tempAesKey)`: Decrypt `OpenBase64Sk` with a 32-byte temp AES key into `OpenSk`. Empty temp key decodes Base64 only.
- `FromJsonOverride(string json)`, `FromJsonString(string json)`, `ToString()`

## RabbitHomeClient

Queries Home `/route` for an available instance. Implements `IDisposable`. Default HTTP timeout is 100 seconds. Query parameter key is `q`.

- Property: `HomeUrl`
- Constructors:
  - `RabbitHomeClient(string homeUrl, bool usePost)`
  - `RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)`
  - `RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost)`
  - `RabbitHomeClient(IHttpClientProxy httpProxyProxy, string homeUrl, bool usePost, TimeSpan timeout)`
- Methods:
  - `SetPublicRsa(IRsaPublicCipher pub)` / `SetPublicRsa(RSA pubKey)`
  - `QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath, string publicKeyContent)`: Load public key from path or content; `KeyError = true` if load fails.
  - `QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)`
  - `QueryFromHome(QueryRouteInfo queryInfo)`: Uses the configured public key; no encryption if unset. `ParamError = true` when `queryInfo` is null.
  - `Dispose()`

The request body is `QueryRouteInfo` JSON, UTF-8 encoded, optionally RSA-encrypted, then encoded as `q` with `RabbitHomeDefaults.Base64Encoding` (default `Base64RawUrlEncoding`).

## RabbitHomeDefaults

Default settings for Home communication.

- `HttpKeyQuery`: `"q"`
- `HttpPatternRoute`: `"/route"`
- `LittleEndian`: default `true`; `SetLittleEndian(bool)`
- `Base64Encoding`: default `Base64RawUrlEncoding`; `SetBase64Encoding(IBase64Encoding)`

## RabbitHomeUtils

Loads the RSA public key used for Home communication. PEM / X509 vs PKCS#1 v1.5 is selected by `isPemKey`. Non-empty content is preferred over path.

- `LoadHomePublicRsa(HomeSettings homeSettings)`
- `LoadHomePublicRsa(bool isPemKey, string pubKeyPath, string pubKeyContent)`
- `LoadHomePublicRsaWithPath(bool isPemKey, string pubKeyPath)`
- `LoadHomePublicRsaWithContent(bool isPemKey, string pubKeyContent)`

Returns `null` when both path and content are empty.
