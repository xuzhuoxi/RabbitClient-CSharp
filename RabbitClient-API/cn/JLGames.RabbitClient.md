# 命名空间：JLGames.RabbitClient

本命名空间为 RabbitClient-CSharp 的统一管理入口，负责协调 Rabbit-Home 查询与 Rabbit-Server 连接。`RabbitClientManager` 继承 `EventDispatcher` 并实现 `IDisposable`。

## RabbitClientManager

统一管理 Home 查询、Socket 连接、AES 会话加密与事件分发。

### 属性

- `HomeSettings`：Rabbit-Home 连接与加密配置。
- `HomeHttpProxy`：用于 Home HTTP 请求的 `IHttpClientProxy`。
- `HomeClient`：`RabbitHomeClient` 实例。
- `QueryInfo`：最近一次提交给 Home 的 `QueryRouteInfo`。
- `SocketServer`：底层 `RabbitSocketServer`。
- `SocketClient`：高层 `RabbitSocketClient`（加密与事件分发）。

### 构造函数

- `RabbitClientManager(IHttpClientProxy homeHttpProxy, string homeUrl, bool usePost, bool enableKey, bool isPemKey, string pubKeyPath, string pubKeyContent)`：按 URL 与密钥选项创建。`pubKeyPath` 与 `pubKeyContent` 二选一即可。
- `RabbitClientManager(IHttpClientProxy homeHttpProxy, HomeSettings homeSettings)`：使用已有 `HomeSettings` 创建。

### 方法

- `SetThreadSocketContext(SynchronizationContext context)`：将 Socket 回调派发到指定同步上下文。
- `ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)`：使用平台、类型与临时 AES 密钥查询并连接。
- `ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "RabbitClient")`：`randomAesKey` 为 true 时用 PBKDF2 从口令派生 32 字节临时 AES 密钥（默认口令 `RabbitClient`）。
- `ConnectThroughHome(QueryRouteInfo queryInfo)`：使用完整查询参数连接。
- `Dispose()`：释放 Socket、Home 客户端与分发器。

连接成功后若 `OpenSk` 非空，会用 AES 设置 `SocketClient` 对称密钥并 `StartReceiving()`。

## RabbitClientManagerEvents

连接流程事件名与载荷类型。

### 事件

| 事件 | 载荷 |
| --- | --- |
| `EventOnProgressHome` | `ProgressEventData<QueryResult>` |
| `EventOnProgressServer` | `ProgressEventData<SocketEvents.SocketConnEventInfo>` |
| `EventOnConnectFinish` | `bool`（是否连接成功） |

### ProgressEventData&lt;T&gt;

通用进度载荷。

- `Suc`：该步骤是否成功。
- `Data`：步骤结果（若有）。
- `Error`：因异常失败时的异常。
- `ToString()`：诊断字符串。
