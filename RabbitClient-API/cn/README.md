# RabbitClient-CSharp API 文档总览

## 概述

RabbitClient-CSharp 是面向 Rabbit-Home / Rabbit-Server 的 .NET Standard 2.0 客户端库。入口为 `JLGames.RabbitClient.RabbitClientManager`：先向 Rabbit-Home 查询路由，再以 Socket 连接对应 Rabbit-Server，并收发加密消息。本文档与当前源码公开 API 对齐。

## 模块列表

### 管理器

- **[JLGames.RabbitClient](JLGames.RabbitClient.md)**
  - `RabbitClientManager`：查询、连接、会话加密、进度事件
  - `RabbitClientManagerEvents`：Home / Server 进度与连接完成事件

### Home 模块

- **[JLGames.RabbitClient.Home](JLGames.RabbitClient.Home.md)**
  - `HomeSettings`、`HomeResponseInfo`
  - `QueryResult`、`QueryRouteInfo`、`QueryRouteBackInfo`
  - `RabbitHomeClient`、`RabbitHomeDefaults`、`RabbitHomeUtils`

### Server 核心模块

- **[JLGames.RabbitClient.Server](JLGames.RabbitClient.Server.md)**
  - `RabbitSocketClient`、`RabbitSocketServer`
  - `RabbitServerDefaults`
  - `RabbitSocketClientEvents`、`RabbitSocketServerEvents`

### 消息协议模块

- **[JLGames.RabbitClient.Server.Message](JLGames.RabbitClient.Server.Message.md)**
  - `IRabbitMessageHeader`、`IRabbitMessageContent`
  - `IRabbitMessageReader`、`IRabbitMessageWriter`
  - `RabbitMessageHeader`、`RabbitMessageReader`、`RabbitMessageWriter`
  - `IRabbitRequestMsg` / `RabbitRequestMsg`
  - `IRabbitResponseMsg` / `RabbitResponseMsg`

### MMO 模块

- **[JLGames.RabbitClient.Server.MMO](JLGames.RabbitClient.Server.MMO.md)**
  - 实体：`IEntity` / `EntityPlayer` / `EntityRoom` / `EntityUnit`
  - 变量：`IVarSet`、`VarSet`、`VarData<T>`、`VarSetDelegates`
  - 管理：`MmoManager`
  - 事件：`PlayerEvents`、`RoomEvents`、`UnitEvents`、`WorldEvents`
  - Meta：`MmoMetas`、`ProtoMMOCode`、`PlayerVarKeys`、`RoomVarKeys`、`UnitVarKeys`
  - 数学：`V2Int`、`V3Int`
  - 枚举：`EntityType`、`VarType`、`CampType`、`RoomType`、`UnitType`

## 快速开始

推荐通过 `RabbitClientManager` 完成发现与连接；也可只使用 Home / Socket 模块。

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

`randomAesKey: true` 时默认用口令 `RabbitClient` 派生 32 字节临时 AES 密钥。

仅查询路由、自行连接：

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

变量集合：

```csharp
IVarSet vars = new VarSet(true);
vars.SetVar("hp", 100);
vars.SetVar("pos", new V3Int(1, 2, 3));
```

### 模块依赖关系

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

## 版本信息

- **库名：** RabbitClient-CSharp
- **目标框架：** .NET Standard 2.0
- **C# 版本：** 7.3+
- **文档：** 与当前源码公开 API 对齐

## 许可证

本文档与相关代码遵循 [MIT](../../LICENSE) 许可证。
