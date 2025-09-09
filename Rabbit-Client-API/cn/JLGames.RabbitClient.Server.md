# 命名空间：JLGames.RabbitClient.Server

本命名空间包含与 Rabbit-Server 连接、事件、默认配置等相关的核心接口与实现。

## 类与接口

### RabbitSocketClient
- 描述：Rabbit-Server 客户端，负责与服务器通信。
- 主要属性：
  - `Connected`：是否已连接。
  - `EventDispatcher`：事件分发器。
- 构造函数：
  - `RabbitSocketClient(RabbitSocketServer socketServer)`：初始化客户端。
- 主要方法：
  - `SetThreadContext(FixedThreadContext context)`：设置线程上下文。
  - `SetSymmetricCipher(ICipher cipher)`：设置对称密钥。
  - `StartReceiving()`：开始接收消息。
  - `StopReceiving()`：停止接收消息。
  - `SendMessage(IRabbitMessageWriter msg)`：发送消息。
  - `Dispose()`：释放资源。

### RabbitSocketServer
- 描述：Rabbit-Server 服务器端，负责与客户端通信。
- 主要属性：
  - `SocketServer`：底层Socket客户端。
  - `Connecting`：是否正在连接或断开连接。
  - `Connected`：是否已连接。
- 构造函数：
  - `RabbitSocketServer()`：初始化服务器。
- 主要方法：
  - `SetThreadContext(FixedThreadContext context)`：设置线程上下文。
  - `ConnectServer(QueryRouteBackInfo serverInfo)`：连接到服务器。
  - `DisconnectServer()`：断开与服务器的连接。

### RabbitServerDefaults
- 描述：Rabbit-Server 相关常量和默认配置。
- 主要属性/方法：
  - `LittleEndian`：字节序设置。
  - `ApmMode`：API连接模式。
  - `SetLittleEndian(bool)`：设置字节序。
  - `SetConnectApiMode(bool)`：设置API连接模式。

## 事件

### RabbitSocketServerEvents
- 事件常量：
  - `EventOnConnectionOpenSuc`：连接成功事件（事件数据：SocketEvents.SocketConnEventInfo）。
  - `EventOnConnectionOpenFail`：连接失败事件（事件数据：SocketEvents.SocketConnEventInfo）。
  - `EventOnConnectionClose`：关闭连接结果事件（事件数据：SocketEvents.SocketConnEventInfo）。
  - `EventOnServerMessage`：数据接收处理完成（事件数据：byte[] message）。

### RabbitSocketClientEvents
- 事件常量：
  - `EventOnClientMessage`：数据接收处理完成（事件数据：IRabbitResponseMsg）。 