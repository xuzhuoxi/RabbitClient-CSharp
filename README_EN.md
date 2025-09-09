# Rabbit-Client-CSharp

## Project Overview
Rabbit-Client-CSharp is a C# client library for accessing Rabbit-Home servers and automatically selecting and connecting to Rabbit-Server instances. This library is suitable for distributed application scenarios that require dynamic discovery and connection to backend services.

## Key Features
- Query available Rabbit-Server instances through Rabbit-Home.
- Support for security mechanisms such as key encryption and Base64 encoding.
- Provide Socket communication capabilities with Rabbit-Server.
- Unified client manager that simplifies connection and message sending/receiving processes.
- Rich event mechanisms for easy integration and extension.

## Directory Structure
```
Rabbit-Client-CSharp/
├── Rabbit-Client-CSharp/           # Main library code
│   └── JLGames/
│       └── RabbitClient/
│           ├── Home/               # APIs related to Rabbit-Home server
│           ├── Server/             # APIs related to Rabbit-Server
│           ├── RabbitClientManager.cs  # Unified manager
│           └── ...
├── Rabbit-Client-Test/             # Test code
├── Rabbit-Client-API/              # Generated API documentation directory
└── ...
```

## Quick Start
1. Reference the `Rabbit-Client-CSharp` project or the compiled DLL.
2. Configure Rabbit-Home address and key parameters through `RabbitClientManager`.
3. Call the `ConnectThroughHome` method to automatically discover and connect to Rabbit-Server.
4. Use `SocketClient` for message sending and receiving.

## Example Code
```csharp
var manager = new RabbitClientManager("http://home.server", true, false, false, null);
await manager.ConnectThroughHome("platformId", "typeName", false);
// After successful connection, communication can be performed through manager.SocketClient
```

## API Documentation

For detailed API documentation, please refer to the following links:

### Chinese Documentation
- **[API 文档总览](Rabbit-Client-API/cn/README.md)** - 完整的中文API文档索引
- **[Home 模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Home.md)** - Rabbit-Home 服务器相关API
- **[Server 核心模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.md)** - Socket通信核心API
- **[消息协议模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.Message.md)** - 消息读写与协议API
- **[MMO模块](Rabbit-Client-API/cn/JLGames.RabbitClient.Server.MMO.md)** - MMO游戏相关API

### English Documentation
- **[API Documentation Overview](Rabbit-Client-API/en/README.md)** - Complete English API documentation index
- **[Home Module](Rabbit-Client-API/en/JLGames.RabbitClient.Home.md)** - Rabbit-Home server related APIs
- **[Server Core Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.md)** - Socket communication core APIs
- **[Message Protocol Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.Message.md)** - Message read/write and protocol APIs
- **[MMO Module](Rabbit-Client-API/en/JLGames.RabbitClient.Server.MMO.md)** - MMO game related APIs

## Use Cases
- Dynamic discovery and connection of game servers
- Distributed service registration and discovery
- Client-server architectures requiring secure communication

## License
This project follows the MIT License.
