# Namespace: JLGames.RabbitClient.Home

This namespace contains APIs related to interacting with Rabbit-Home servers.

## Classes and Structures

### HomeResponseInfo
- Description: Response information returned by Rabbit-Home.
- Main Properties:
  - `ExtCode`: Extended status code.
  - `Info`: Return information, usually a base64 encoded json string.
  - `Other`: Other return information, usually a base64 encoded json string.
- Main Methods:
  - `FromJsonOverride(string json)`: Parse from json string.
  - `FromJsonString(string json)`: Static method to parse from json string.

### HomeSettings
- Description: Configuration information for Rabbit-Home server.
- Main Properties:
  - `HomeUrl`: Rabbit-Home server address.
  - `UsePost`: Whether to use POST requests.
  - `EnableKey`: Whether to enable key.
  - `IsPemKey`: Whether the key is in PEM format.
  - `PublicKeyPath`: Public key path.
- Constructor:
  - `HomeSettings(string homeUrl, bool usePost, bool enableKey, bool isPemKey, string publicKeyPath)`: Initialize configuration information.

### QueryResult
- Description: Query Rabbit-Home result.
- Main Properties:
  - `Ok`: Whether successful.
  - `KeyError`: Key error.
  - `ParamError`: Parameter error.
  - `TimeOut`: Timeout.
  - `SucInfo`: Server information returned on success (`QueryRouteBackInfo`).
  - `FailInfo`: Information returned on failure (`HomeResponseInfo`).
- Main Methods:
  - `ToString()`: Return structure content string.

### QueryRouteInfo
- Description: Used to query available Rabbit-Server instances from Rabbit-Home.
- Main Properties:
  - `PlatformId`: Service platform Id.
  - `TypeName`: Type name.
  - `TempAesKey`: Temporary AES key for encrypting data returned by Rabbit-Home. If not provided, the returned key data will be returned as a Base64 string.
- Main Methods:
  - `ToJsonString()`: Serialize to json string.
  - `ToString()`: Return content string.

### QueryRouteBackInfo
- Description: Available Rabbit-Server instance information returned by Rabbit-Home.
- Main Properties:
  - `Id`: Instance unique Id.
  - `PlatformId`: Service platform Id.
  - `TypeName`: Instance type name.
  - `OpenNetwork`: Open connection protocol.
  - `OpenAddr`: Open connection address.
  - `OpenKeyOn`: Whether key verification is enabled.
  - `OpenBase64Sk`: Base64 string of temporary key.
  - `OpenSk`: Temporary key byte array, updated after executing ComputeOpenSk.
- Main Methods:
  - `ComputeOpenSk(byte[] tempAesKey)`: Decrypt communication key using temporary AES key.
  - `FromJsonOverride(string json)`: Parse from json string.
  - `FromJsonString(string json)`: Static method to parse from json string.
  - `ToString()`: Return content string.

### RabbitHomeClient
- Description: Rabbit-Home client, responsible for communicating with Rabbit-Home server and querying available Rabbit-Server instances.
- Constructors:
  - `RabbitHomeClient(string homeUrl, bool usePost)`: Initialize client.
  - `RabbitHomeClient(string homeUrl, bool usePost, TimeSpan timeout)`: Initialize client and set timeout.
- Main Methods:
  - `SetPublicRsa(IRsaPublicCipher pub)`: Set RSA public key.
  - `SetPublicRsa(RSA pubKey)`: Set RSA public key.
  - `QueryFromHome(QueryRouteInfo queryInfo, bool isPemKey, string publicKeyPath)`: Query available instances from Rabbit-Home (supports PEM/PKCS1 public key files).
  - `QueryFromHome(QueryRouteInfo queryInfo, IRsaPublicCipher publicCipher)`: Query available instances from Rabbit-Home (custom public key implementation).
  - `QueryFromHome(QueryRouteInfo queryInfo)`: Query available instances from Rabbit-Home.

### RabbitHomeDefaults
- Description: Rabbit-Home related constants and default configuration.
- Main Properties/Methods:
  - `HttpKeyQuery`: Query parameter name.
  - `HttpPatternRoute`: Route path.
  - `LittleEndian`: Byte order setting.
  - `Base64Encoding`: Base64 encoding implementation.
  - `SetLittleEndian(bool)`: Set byte order.
  - `SetBase64Encoding(IBase64Encoding)`: Set Base64 encoding implementation.
