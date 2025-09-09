# Rabbit-Client-CSharp API 文档总览

## 概述

Rabbit-Client-CSharp 是一套面向分布式游戏服务器的 C# 客户端框架，支持与 Rabbit-Home、Rabbit-Server 等服务的高效通信，适用于 MMO、房间、实体、消息等多种场景。本文档涵盖了所有核心模块的 API 参考。

## 模块列表

### Home 模块
- **[JLGames.RabbitClient.Home](JLGames.RabbitClient.Home.md)**
  - HomeResponseInfo - Home服务器响应信息结构
  - HomeSettings - Home服务器配置信息
  - QueryResult, QueryRouteInfo, QueryRouteBackInfo - 查询与路由相关结构
  - RabbitHomeClient - Home服务客户端
  - RabbitHomeDefaults - Home相关常量与配置

### Server 核心模块
- **[JLGames.RabbitClient.Server](JLGames.RabbitClient.Server.md)**
  - RabbitSocketClient, RabbitSocketServer - Socket通信核心
  - RabbitServerDefaults - 服务器默认配置
  - RabbitSocketServerEvents, RabbitSocketClientEvents - 连接与消息事件

### 消息协议模块
- **[JLGames.RabbitClient.Server.Message](JLGames.RabbitClient.Server.Message.md)**
  - IRabbitMessage, IRabbitMessageReader, IRabbitMessageWriter - 消息接口
  - RabbitMessageReader, RabbitMessageWriter - 消息读写实现
  - IRabbitRequestMsg, RabbitRequestMsg - 请求消息接口与实现
  - IRabbitResponseMsg, RabbitResponseMsg - 响应消息接口与实现
  - RabbitHeader - 消息头结构

### MMO/实体/变量/事件模块
- **[JLGames.RabbitClient.Server.MMO](JLGames.RabbitClient.Server.MMO.md)**
  - Entity/EntityPlayer/EntityRoom/EntityUnit - MMO实体结构
  - IVarSet, VarSet - 变量集合接口与实现
  - MmoManager, MmoSettings - MMO管理与配置
  - 各类事件（PlayerEvents, RoomEvents, UnitEvents, WorldEvents）
  - 元数据与常量（MmoMetas, ProtoMMOCode, PlayerVarKeys, RoomVarKeys, UnitVarKeys 等）
  - 数学结构体（V2Int, V3Int 等）
  - 枚举类型（EntityType, VarType, CampType, RoomType, UnitType 等）

## 快速开始

### 基本使用示例

```csharp
// 1. 查询Rabbit-Home获取可用服务器
var homeClient = new RabbitHomeClient("http://home.server", true);
var queryInfo = new QueryRouteInfo { PlatformId = "game", TypeName = "MMO" };
var result = await homeClient.QueryFromHome(queryInfo);
if (result.Ok) {
    var serverInfo = result.SucInfo;
    // ...
}

// 2. 连接Rabbit-Server并发送消息
var server = new RabbitSocketServer();
server.ConnectServer(serverInfo);
var client = new RabbitSocketClient(server);
client.StartReceiving();
client.SendMessage(new RabbitRequestMsg());

// 3. 变量操作
IVarSet vars = new VarSet(true);
vars.SetVar("hp", 100);
vars.SetVar("pos", new V3Int(1,2,3));
```

### 模块依赖关系

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

### API



## 版本信息

- **框架版本：** Rabbit-Client-CSharp
- **目标框架：** .NET Standard 2.0+
- **C#版本：** 7.3+
- **文档版本：** 1.0

## 贡献指南

如需贡献代码或改进文档，请遵循以下规范：

1. 保持代码风格一致
2. 添加适当的XML注释
3. 包含中文和英文注释
4. 提供使用示例
5. 更新相关文档

## 许可证

本文档和相关代码遵循相应的开源许可证。

---

**注意：** 本文档涵盖了Rabbit-Client-CSharp框架的所有主要模块。每个模块都有详细的API文档，包含接口定义、类说明、方法描述和使用示例。建议根据具体需求查阅相应的模块文档。 