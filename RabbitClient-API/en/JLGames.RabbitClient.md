# Namespace: JLGames.RabbitClient

Unified entry for RabbitClient-CSharp. Coordinates Rabbit-Home queries and Rabbit-Server connections. `RabbitClientManager` extends `EventDispatcher` and implements `IDisposable`.

## RabbitClientManager

Orchestrates home route query, socket connection, AES session encryption, and event dispatch.

### Properties

- `HomeSettings`: Rabbit-Home connection and crypto settings.
- `HomeHttpProxy`: `IHttpClientProxy` used for Home HTTP requests.
- `HomeClient`: `RabbitHomeClient` instance.
- `QueryInfo`: Last `QueryRouteInfo` submitted to Home.
- `SocketServer`: Low-level `RabbitSocketServer`.
- `SocketClient`: High-level `RabbitSocketClient` (encryption and event dispatch).

### Constructors

- `RabbitClientManager(IHttpClientProxy homeHttpProxy, string homeUrl, bool usePost, bool enableKey, bool isPemKey, string pubKeyPath, string pubKeyContent)`: Create from URL and key options. Provide either `pubKeyPath` or `pubKeyContent`.
- `RabbitClientManager(IHttpClientProxy homeHttpProxy, HomeSettings homeSettings)`: Create from an existing `HomeSettings`.

### Methods

- `SetThreadSocketContext(SynchronizationContext context)`: Dispatch socket callbacks onto the given synchronization context.
- `ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)`: Query and connect using platform, type, and a temporary AES key.
- `ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "RabbitClient")`: When `randomAesKey` is true, derive a 32-byte temporary AES key via PBKDF2 (default passphrase `RabbitClient`).
- `ConnectThroughHome(QueryRouteInfo queryInfo)`: Connect with full query parameters.
- `Dispose()`: Release socket, home client, and dispatcher resources.

After a successful connect, if `OpenSk` is present, the manager sets an AES cipher on `SocketClient` and calls `StartReceiving()`.

## RabbitClientManagerEvents

Event names and payload types for the connection flow.

### Events

| Event | Payload |
| --- | --- |
| `EventOnProgressHome` | `ProgressEventData<QueryResult>` |
| `EventOnProgressServer` | `ProgressEventData<SocketEvents.SocketConnEventInfo>` |
| `EventOnConnectFinish` | `bool` (whether the connection succeeded) |

### ProgressEventData&lt;T&gt;

Generic progress payload.

- `Suc`: Whether this step succeeded.
- `Data`: Step result when available.
- `Error`: Exception when the step failed with an error.
- `ToString()`: Diagnostic string.
