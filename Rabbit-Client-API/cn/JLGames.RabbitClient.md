# 命名空间：JLGames.RabbitClient

本命名空间为 Rabbit-Client-CSharp 的统一管理入口，负责协调 Rabbit-Home 与 Rabbit-Server 的连接与通信。

## 类与事件

### RabbitClientManager
- 描述：统一管理 Rabbit-Home 查询与 Rabbit-Server 连接的客户端管理器。
- 主要属性：
  - `HomeSettings`：Rabbit-Home 配置信息。
  - `QueryInfo`：当前查询信息。
  - `HomeClient`：Rabbit-Home 客户端。
  - `SocketServer`：Rabbit-Server 通信服务端。
  - `SocketClient`：Rabbit-Server 通信客户端。
- 主要方法：
  - `ConnectThroughHome(string platformId, string typeName, byte[] tempAesKey)`：通过 Rabbit-Home 查询并连接服务器。
  - `ConnectThroughHome(string platformId, string typeName, bool randomAesKey, string passphrase = "Rabbit-Client")`：通过 Rabbit-Home 查询并连接服务器，支持随机密钥。
  - `ConnectThroughHome(QueryRouteInfo queryInfo)`：通过自定义查询信息连接。

### RabbitClientManagerEvents
- 描述：RabbitClientManager 相关事件常量。
- 主要事件：
  - `EventOnProgressHome`：连接进度事件（Rabbit-Home 查询阶段）。
  - `EventOnProgressServer`：连接进度事件（Rabbit-Server 连接阶段）。
  - `EventOnConnectFinish`：连接完成事件。
- 事件数据类型：
  - `ProgressEventData<T>`：进度事件数据，包含 `Suc`（是否成功）和 `Data`（相关数据）。 