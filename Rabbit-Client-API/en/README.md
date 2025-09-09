# Rabbit-Client-CSharp API Documentation Overview

## Overview

Rabbit-Client-CSharp is a C# client framework designed for distributed game servers, supporting efficient communication with Rabbit-Home, Rabbit-Server, and other services. It is suitable for various scenarios including MMO, rooms, entities, and messaging. This documentation covers API references for all core modules.

## Module List

### Home Module
- **[JLGames.RabbitClient.Home](JLGames.RabbitClient.Home.md)**
  - HomeResponseInfo - Home server response information structure
  - HomeSettings - Home server configuration information
  - QueryResult, QueryRouteInfo, QueryRouteBackInfo - Query and routing related structures
  - RabbitHomeClient - Home service client
  - RabbitHomeDefaults - Home related constants and configuration

### Server Core Module
- **[JLGames.RabbitClient.Server](JLGames.RabbitClient.Server.md)**
  - RabbitSocketClient, RabbitSocketServer - Socket communication core
  - RabbitServerDefaults - Server default configuration
  - RabbitSocketServerEvents, RabbitSocketClientEvents - Connection and message events

### Message Protocol Module
- **[JLGames.RabbitClient.Server.Message](JLGames.RabbitClient.Server.Message.md)**
  - IRabbitMessage, IRabbitMessageReader, IRabbitMessageWriter - Message interfaces
  - RabbitMessageReader, RabbitMessageWriter - Message read/write implementations
  - IRabbitRequestMsg, RabbitRequestMsg - Request message interface and implementation
  - IRabbitResponseMsg, RabbitResponseMsg - Response message interface and implementation
  - RabbitHeader - Message header structure

### MMO/Entity/Variable/Event Module
- **[JLGames.RabbitClient.Server.MMO](JLGames.RabbitClient.Server.MMO.md)**
  - Entity/EntityPlayer/EntityRoom/EntityUnit - MMO entity structures
  - IVarSet, VarSet - Variable collection interface and implementation
  - MmoManager, MmoSettings - MMO management and configuration
  - Various events (PlayerEvents, RoomEvents, UnitEvents, WorldEvents)
  - Metadata and constants (MmoMetas, ProtoMMOCode, PlayerVarKeys, RoomVarKeys, UnitVarKeys, etc.)
  - Mathematical structures (V2Int, V3Int, etc.)
  - Enumeration types (EntityType, VarType, CampType, RoomType, UnitType, etc.)

## Quick Start

### Basic Usage Example

```csharp
// 1. Query Rabbit-Home to get available servers
var homeClient = new RabbitHomeClient("http://home.server", true);
var queryInfo = new QueryRouteInfo { PlatformId = "game", TypeName = "MMO" };
var result = await homeClient.QueryFromHome(queryInfo);
if (result.Ok) {
    var serverInfo = result.SucInfo;
    // ...
}

// 2. Connect to Rabbit-Server and send messages
var server = new RabbitSocketServer();
server.ConnectServer(serverInfo);
var client = new RabbitSocketClient(server);
client.StartReceiving();
client.SendMessage(new RabbitRequestMsg());

// 3. Variable operations
IVarSet vars = new VarSet(true);
vars.SetVar("hp", 100);
vars.SetVar("pos", new V3Int(1,2,3));
```

### Module Dependencies

```
RabbitClient
├── Home
├── Server
│   ├── Message
│   │   ├── Request
│   │   └── Response
│   └── MMO
│       ├── Entity
│       ├── Event
│       ├── Lang
│       ├── Meta
│       └── Var
```

## Version Information

- **Framework Version:** Rabbit-Client-CSharp
- **Target Framework:** .NET Standard 2.0+
- **C# Version:** 7.3+
- **Documentation Version:** 1.0

## Contributing Guidelines

If you want to contribute code or improve documentation, please follow these standards:

1. Maintain consistent code style
2. Add appropriate XML comments
3. Include both Chinese and English comments
4. Provide usage examples
5. Update related documentation

## License

This documentation and related code follow the corresponding open source license.

---

**Note:** This documentation covers all major modules of the Rabbit-Client-CSharp framework. Each module has detailed API documentation, including interface definitions, class descriptions, method descriptions, and usage examples. It is recommended to refer to the corresponding module documentation based on specific needs.
