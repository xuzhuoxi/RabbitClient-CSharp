# 命名空间：JLGames.RabbitClient.Server

Rabbit-Server 的 Socket 连接、收发、默认配置与事件。

## RabbitSocketClient

高层客户端：对称加密、按 Extension 分发、发送/接收事件。实现 `IDisposable`。线程上下文由绑定的 `RabbitSocketServer` 设置。

- 属性：
  - `Connected`：底层 Socket 是否已连接。
  - `EventDispatcher`：客户端生命周期事件分发器。
- 构造函数：`RabbitSocketClient(RabbitSocketServer socketServer)`
- 方法：
  - `GetExtensionDispatcher(string extensionName)`：按扩展名取得或创建 `IEventDispatcher`。收到消息后以消息头 `ProtoId` 为事件名派发 `RabbitResponseMsg`。
  - `SetSymmetricCipher(ICipher cipher)`：设置对称加密器（通常为 AES / `OpenSk`）。
  - `StartReceiving()` / `StopReceiving()`：开始或停止监听 `EventOnServerMessage`。
  - `SendMessage(IRabbitMessageWriter msg)`：编码、可选加密后发送。发送前抛出 `EventOnClientSendMessagePrepare`，发送后抛出 `EventOnClientSendMessage`。
  - `Dispose()`

## RabbitSocketServer

底层连接封装，继承 `EventDispatcher`。按 `QueryRouteBackInfo.OpenNetwork` / `OpenAddr` 建立 Socket。

- 属性：
  - `SocketServer`：底层 `ISocketClient`。
  - `Connecting`：是否正在连接或断开。
  - `Connected`：是否已连接。
- 构造函数：`RabbitSocketServer()`
- 方法：
  - `SetThreadSocketContext(SynchronizationContext context)`：设置 Socket 回调同步上下文。
  - `ConnectServer(QueryRouteBackInfo serverInfo)`：连接服务器；已在连接中或已连接则忽略。
  - `DisconnectServer()`：断开连接。
  - `Dispose()`：断开并释放。

连接使用 `RabbitServerDefaults.LittleEndian` 与 `ApmMode`。

## RabbitServerDefaults

- `LittleEndian`：默认 `true`；`SetLittleEndian(bool)`
- `ApmMode`：是否使用 APM 连接模式；`SetConnectApiMode(bool apmMode)`

## RabbitSocketServerEvents

| 事件 | 载荷 |
| --- | --- |
| `EventOnConnectionOpenSuc` | `SocketEvents.SocketConnEventInfo` |
| `EventOnConnectionOpenFail` | `SocketEvents.SocketConnEventInfo` |
| `EventOnConnectionClose` | `SocketEvents.SocketConnEventInfo` |
| `EventOnServerMessage` | `byte[]`（原始消息字节） |

## RabbitSocketClientEvents

| 事件 | 载荷 |
| --- | --- |
| `EventOnClientSendMessagePrepare` | `MessageContent`（加密前） |
| `EventOnClientSendMessage` | `MessageContent`（加密后，若已设置密钥） |
| `EventOnClientReceiveMessage` | `IRabbitResponseMsg` |
| `EventOnClientReceiveMessageFailed` | `FailedInfo` |

### MessageContent

实现 `IRabbitMessageContent`。

- 属性：`Extension`、`ProtoId`、`ClientId`、`ProtoUid`、`Content`、`ContentString`
- 方法：`SetHeaderInfo(string extension, string protoId, string cid)`、`SetMessageContent(byte[] content)`

### FailedInfo

收发失败详情（结构体）。

- `IsSend`：是否为发送失败。
- `FailedCode`：失败代码。
- `OriginalBytes`：原始字节。
- `FailedException`：异常。
- `ToString()`
