# Namespace: JLGames.RabbitClient.Server

Socket connection, send/receive, defaults, and events for Rabbit-Server.

## RabbitSocketClient

High-level client: symmetric encryption, per-extension dispatch, send/receive events. Implements `IDisposable`. Thread context is set on the bound `RabbitSocketServer`.

- Properties:
  - `Connected`: Whether the underlying socket is connected.
  - `EventDispatcher`: Dispatcher for client lifecycle events.
- Constructor: `RabbitSocketClient(RabbitSocketServer socketServer)`
- Methods:
  - `GetExtensionDispatcher(string extensionName)`: Get or create an `IEventDispatcher` for an extension. Incoming messages are dispatched with the header `ProtoId` as the event name and a `RabbitResponseMsg` payload.
  - `SetSymmetricCipher(ICipher cipher)`: Set the symmetric cipher (typically AES / `OpenSk`).
  - `StartReceiving()` / `StopReceiving()`: Start or stop listening for `EventOnServerMessage`.
  - `SendMessage(IRabbitMessageWriter msg)`: Encode, optionally encrypt, and send. Dispatches `EventOnClientSendMessagePrepare` before send and `EventOnClientSendMessage` after send.
  - `Dispose()`

## RabbitSocketServer

Low-level connection wrapper, extends `EventDispatcher`. Connects using `QueryRouteBackInfo.OpenNetwork` / `OpenAddr`.

- Properties:
  - `SocketServer`: Underlying `ISocketClient`.
  - `Connecting`: Whether a connect or disconnect is in progress.
  - `Connected`: Whether currently connected.
- Constructor: `RabbitSocketServer()`
- Methods:
  - `SetThreadSocketContext(SynchronizationContext context)`: Set the synchronization context for socket callbacks.
  - `ConnectServer(QueryRouteBackInfo serverInfo)`: Connect to the server; ignored if already connecting or connected.
  - `DisconnectServer()`: Disconnect.
  - `Dispose()`: Disconnect and release.

Connection uses `RabbitServerDefaults.LittleEndian` and `ApmMode`.

## RabbitServerDefaults

- `LittleEndian`: default `true`; `SetLittleEndian(bool)`
- `ApmMode`: whether APM connect mode is used; `SetConnectApiMode(bool apmMode)`

## RabbitSocketServerEvents

| Event | Payload |
| --- | --- |
| `EventOnConnectionOpenSuc` | `SocketEvents.SocketConnEventInfo` |
| `EventOnConnectionOpenFail` | `SocketEvents.SocketConnEventInfo` |
| `EventOnConnectionClose` | `SocketEvents.SocketConnEventInfo` |
| `EventOnServerMessage` | `byte[]` (raw message bytes) |

## RabbitSocketClientEvents

| Event | Payload |
| --- | --- |
| `EventOnClientSendMessagePrepare` | `MessageContent` (before encryption) |
| `EventOnClientSendMessage` | `MessageContent` (after encryption, if a cipher is set) |
| `EventOnClientReceiveMessage` | `IRabbitResponseMsg` |
| `EventOnClientReceiveMessageFailed` | `FailedInfo` |

### MessageContent

Implements `IRabbitMessageContent`.

- Properties: `Extension`, `ProtoId`, `ClientId`, `ProtoUid`, `Content`, `ContentString`
- Methods: `SetHeaderInfo(string extension, string protoId, string cid)`, `SetMessageContent(byte[] content)`

### FailedInfo

Details when send or receive fails (struct).

- `IsSend`: Whether this was a send failure.
- `FailedCode`: Failure code.
- `OriginalBytes`: Original bytes.
- `FailedException`: Exception.
- `ToString()`
