# RabbitClient-CSharp API Documentation Overview

## Overview

RabbitClient-CSharp is a .NET Standard 2.0 client library for Rabbit-Home / Rabbit-Server. The entry point is `JLGames.RabbitClient.RabbitClientManager`: query a route from Rabbit-Home, connect to the matching Rabbit-Server over Socket, and send/receive encrypted messages. This documentation matches the current public API.

## Module List

### Manager

- **[JLGames.RabbitClient](JLGames.RabbitClient.md)**
  - `RabbitClientManager`: query, connect, session encryption, progress events
  - `RabbitClientManagerEvents`: Home / Server progress and connect-finish events

### Home Module

- **[JLGames.RabbitClient.Home](JLGames.RabbitClient.Home.md)**
  - `HomeSettings`, `HomeResponseInfo`
  - `QueryResult`, `QueryRouteInfo`, `QueryRouteBackInfo`
  - `RabbitHomeClient`, `RabbitHomeDefaults`, `RabbitHomeUtils`

### Server Core Module

- **[JLGames.RabbitClient.Server](JLGames.RabbitClient.Server.md)**
  - `RabbitSocketClient`, `RabbitSocketServer`
  - `RabbitServerDefaults`
  - `RabbitSocketClientEvents`, `RabbitSocketServerEvents`

### Message Protocol Module

- **[JLGames.RabbitClient.Server.Message](JLGames.RabbitClient.Server.Message.md)**
  - `IRabbitMessageHeader`, `IRabbitMessageContent`
  - `IRabbitMessageReader`, `IRabbitMessageWriter`
  - `RabbitMessageHeader`, `RabbitMessageReader`, `RabbitMessageWriter`
  - `IRabbitRequestMsg` / `RabbitRequestMsg`
  - `IRabbitResponseMsg` / `RabbitResponseMsg`

### MMO Module

- **[JLGames.RabbitClient.Server.MMO](JLGames.RabbitClient.Server.MMO.md)**
  - Entities: `IEntity` / `EntityPlayer` / `EntityRoom` / `EntityUnit`
  - Variables: `IVarSet`, `VarSet`, `VarData<T>`, `VarSetDelegates`
  - Manager: `MmoManager`
  - Events: `PlayerEvents`, `RoomEvents`, `UnitEvents`, `WorldEvents`
  - Meta: `MmoMetas`, `ProtoMMOCode`, `PlayerVarKeys`, `RoomVarKeys`, `UnitVarKeys`
  - Math: `V2Int`, `V3Int`
  - Enums: `EntityType`, `VarType`, `CampType`, `RoomType`, `UnitType`

## Quick Start

Prefer `RabbitClientManager` for discovery and connection. Home and Socket modules can also be used on their own.

```csharp
using System.Threading;
using System.Threading.Tasks;
using JLGames.Infra.Net;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Home;
using JLGames.RabbitClient.Server.Message;

var homeUrl = "http://127.0.0.1:9000";
var httpProxy = new HttpClientProxy(homeUrl);
var manager = new RabbitClientManager(
    httpProxy,
    homeUrl,
    usePost: false,
    enableKey: true,
    isPemKey: true,
    pubKeyPath: "x509_public.pem",
    pubKeyContent: null);

manager.SetThreadSocketContext(SynchronizationContext.Current);
manager.AddEventListener(RabbitClientManagerEvents.EventOnConnectFinish, evd =>
{
    if (!(bool)evd.Data) return;

    manager.SocketClient.GetExtensionDispatcher("Mmo")
        .AddEventListener("PER", e => { /* RabbitResponseMsg */ });

    var req = new RabbitRequestMsg();
    req.SetClientId("cid");
    req.SetProtoInfo("Mmo", "PER");
    req.StartWriteData();
    req.WriteRequestBase("hello");
    manager.SocketClient.SendMessage(req);
});

await manager.ConnectThroughHome("main01", "Rabbit-Server", randomAesKey: true);
```

When `randomAesKey` is `true`, a 32-byte temporary AES key is derived with passphrase `RabbitClient` by default.

Query a route and connect yourself:

```csharp
var homeClient = new RabbitHomeClient("http://127.0.0.1:9000", usePost: false);
var queryInfo = new QueryRouteInfo { PlatformId = "main01", TypeName = "Rabbit-Server" };
var result = await homeClient.QueryFromHome(queryInfo);
if (result.Ok)
{
    var server = new RabbitSocketServer();
    server.ConnectServer(result.SucInfo);
}
```

Variable set:

```csharp
IVarSet vars = new VarSet(true);
vars.SetVar("hp", 100);
vars.SetVar("pos", new V3Int(1, 2, 3));
```

### Module Dependencies

```
JLGames.RabbitClient
├── Home
├── Server
│   ├── Message
│   └── MMO
│       ├── Entity
│       ├── Event
│       ├── Lang
│       ├── Meta
│       └── Var
```

## Version Information

- **Library:** RabbitClient-CSharp
- **Target framework:** .NET Standard 2.0
- **C# version:** 7.3+
- **Documentation:** aligned with the current public API

## License

This documentation and related code are released under the [MIT](../../LICENSE) license.
