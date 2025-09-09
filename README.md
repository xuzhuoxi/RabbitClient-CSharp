# Rabbit-Client-CSharp

## 项目简介
Rabbit-Client-CSharp 是一个用于访问 Rabbit-Home 服务器并自动选择、连接 Rabbit-Server 实例的 C# 客户端库。该库适用于需要动态发现和连接后端服务的分布式应用场景。

## 主要功能
- 通过 Rabbit-Home 查询可用的 Rabbit-Server 实例。
- 支持密钥加密、Base64 编码等安全机制。
- 提供与 Rabbit-Server 的 Socket 通信能力。
- 统一的客户端管理器，简化连接与消息收发流程。
- 丰富的事件机制，便于集成与扩展。

## 目录结构
```
Rabbit-Client-CSharp/
├── Rabbit-Client-CSharp/           # 主库代码
│   └── JLGames/
│       └── RabbitClient/
│           ├── Home/               # 与 Rabbit-Home 服务器相关的API
│           ├── Server/             # 与 Rabbit-Server 相关的API
│           ├── RabbitClientManager.cs  # 统一管理器
│           └── ...
├── Rabbit-Client-Test/             # 测试代码
├── Rabbit-Client-API/              # 生成的API文档目录
└── ...
```

## 快速开始
1. 引用 `Rabbit-Client-CSharp` 项目或编译生成的 DLL。
2. 通过 `RabbitClientManager` 配置 Rabbit-Home 地址及密钥参数。
3. 调用 `ConnectThroughHome` 方法自动发现并连接 Rabbit-Server。
4. 使用 `SocketClient` 进行消息收发。

## 示例代码
```csharp
var manager = new RabbitClientManager("http://home.server", true, false, false, null);
await manager.ConnectThroughHome("platformId", "typeName", false);
// 连接成功后可通过 manager.SocketClient 进行通信
```

## API 文档

详细的API文档请参考以下链接：

### 中文文档
- **[API 文档总览](Rabbit-Client-API/cn/README.md)** - 完整的中文API文档索引
- **[Home 模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Home.md)** - Rabbit-Home 服务器相关API
- **[Server 核心模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.md)** - Socket通信核心API
- **[消息协议模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.Message.md)** - 消息读写与协议API
- **[MMO模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.MMO.md)** - MMO游戏相关API

### 英文文档
- **[API Documentation Overview](Rabbit-Client-API/en/README.md)** - Complete English API documentation index
- **[Home Module](Rabbit-Client-API/en/JLGames.RabbitClient.Home.md)** - Rabbit-Home server related APIs
- **[Server Core Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.md)** - Socket communication core APIs
- **[Message Protocol Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.Message.md)** - Message read/write and protocol APIs
- **[MMO Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.MMO.md)** - MMO game related APIs

## 适用场景
- 游戏服务器动态发现与连接
- 分布式服务注册与发现
- 需要安全通信的客户端-服务端架构

## 许可证
本项目遵循 MIT 许可证。 