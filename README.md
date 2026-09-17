# RabbitClient-CSharp

[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![C#](https://img.shields.io/badge/C%23-7.3+-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![CI](https://github.com/xuzhuoxi/RabbitClient-CSharp/actions/workflows/CI.yml/badge.svg)](https://github.com/xuzhuoxi/RabbitClient-CSharp/actions/workflows/CI.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

简体中文 | [English](README_EN.md)

.NET Standard 2.0 客户端库：经 Rabbit-Home 发现可用的 Rabbit-Server，再用 Socket 连接并收发加密消息。旁路依赖 [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp)。

## 目录

- [概述](#概述)
- [注意事项](#注意事项)
- [功能特性](#功能特性)
- [技术栈](#技术栈)
- [目录结构](#目录结构)
- [安装](#安装)
- [快速开始](#快速开始)
- [模块](#模块)
- [API 文档](#api-文档)
- [构建与测试](#构建与测试)
- [发版](#发版)
- [许可证](#许可证)

## 概述

RabbitClient-CSharp 面向需要动态发现后端的客户端：先向 Rabbit-Home 查询路由，再连接对应的 Rabbit-Server 实例。入口是 `JLGames.RabbitClient.RabbitClientManager`，内部协调 Home 查询、Socket 连接、AES 会话加密与事件分发。

主库目标框架为 **.NET Standard 2.0**（C# 7.3），可在 Windows、macOS、Linux 上使用。测试项目为 **net8.0**。

当前以 GitHub Release 的 Debug / Release DLL zip 分发；NuGet 打包尚未启用。

## 注意事项

- **与 Infra-CSharp 对齐版本。** 本地源码构建时，本仓库与 [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp) 须位于同一父目录（`JLGameStudios/Infra-CSharp` 与 `JLGameStudios/RabbitClient-CSharp`）。CI / Release 不固定检 `master`，而是读根目录 `Require.yml`：在 `Require` 数组中找到 `Tag` 等于本仓库 tag 的项，再用该项的 `Infra-CSharp` 检出依赖。找不到对应项时工作流会中止。
- **与 Rabbit-Home / Rabbit-Server 互通时请使用匹配的服务端版本。** 路由查询、消息分帧和会话密钥随协议演进，混用版本可能导致无法握手或解析失败。
- 依赖本机 Rabbit-Home（默认 HTTP `127.0.0.1:9000`）和 Rabbit-Server 的用例标了 `RunOnlyThis`，CI 中会跳过。
- 公开 API 以源码为准。线程上下文类型为 `SynchronizationContext`。

## 功能特性

- **Home 发现**：`RabbitHomeClient` 向 Rabbit-Home 查询可用 Server（GET/POST、可选 RSA 公钥加密查询参数）。
- **统一管理器**：`RabbitClientManager.ConnectThroughHome` 完成查询、连接、设置 AES 会话密钥并开始接收。
- **Socket 通信**：`RabbitSocketServer` / `RabbitSocketClient` 连接、收发、按 Extension 名称取得 `IEventDispatcher`。
- **消息协议**：`JLGames.RabbitClient.Server.Message` 下的请求 / 响应读写（`RabbitRequestMsg` / `RabbitResponseMsg`）。
- **加密会话**：Home 返回的 `OpenSk` 用于 AES；查询阶段可选 RSA。
- **事件**：连接进度（Home / Server）、连接完成、发送前后、收发失败、MMO 房间/玩家/单位事件。
- **MMO**：实体（Player / Room / Unit）、变量集 `IVarSet`、Meta 与事件。
- **线程**：`SetThreadSocketContext(SynchronizationContext)` 将 Socket 回调派发到指定上下文。

## 技术栈

- **.NET Standard 2.0** / **C# 7.3+**（测试为 C# 10 / net8.0）
- **NUnit 3**：单元测试
- **Infra-CSharp**：旁路 `ProjectReference`（网络、事件、加密等），无其它必选 NuGet 包
- **dotnet CLI**：构建与测试

## 目录结构

```
RabbitClient-CSharp/
├── RabbitClient/                 # 主库（netstandard2.0）
│   └── JLGames/RabbitClient/
│       ├── Home/                 # Rabbit-Home 查询
│       ├── Server/               # Socket、消息、MMO
│       ├── RabbitClientManager.cs
│       └── RabbitClientManagerEvents.cs
├── RabbitClient-Test/            # 测试（net8.0）
├── RabbitClient-API/             # 中英 API 文档
├── notes/release/                # 发行说明模板与各 tag 说明
├── Require.yml                   # 本仓库 tag → Infra-CSharp tag
├── .github/workflows/            # CI / Release / ReleaseNote
└── RabbitClient-CSharp.sln
```

解决方案还引用旁路项目 `../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`。

## 安装

### 前置要求

- 兼容 .NET Standard 2.0 的运行时
- 从源码构建与跑测试：.NET 8 SDK；本地联调还需 Rabbit-Home / Rabbit-Server

### 从 GitHub Release 引用 DLL

在 [GitHub Releases](https://github.com/xuzhuoxi/RabbitClient-CSharp/releases) 下载与目标 tag 对应的 zip（文件名形如 `RabbitClient-CSharp_<tag>_release_netstandard2.0.zip` 或 `..._debug_...`），解压后引用 `RabbitClient.dll`。包内同时包含 `Infra-CSharp.dll`、`.pdb`、`.deps.json` 与许可证/说明。

### 从源码构建

```bash
git clone https://github.com/xuzhuoxi/RabbitClient-CSharp.git
git clone https://github.com/xuzhuoxi/Infra-CSharp.git
# 两个仓库须在同一父目录下
cd RabbitClient-CSharp
dotnet build RabbitClient-CSharp.sln --configuration Release
```

本地 Infra 的检出 tag 应与 `Require.yml` 中当前本仓库版本对应项一致。例如本仓库 `v1.2.1` 对应 Infra `v1.4.1`。

## 快速开始

```csharp
using System.Threading;
using System.Threading.Tasks;
using JLGames.Infra.Net;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Home;

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
    var ok = (bool)evd.Data;
    if (ok)
    {
        // 使用 manager.SocketClient.SendMessage(...) 收发
        // 使用 manager.SocketClient.GetExtensionDispatcher(name) 订阅 Extension 事件
    }
});

await manager.ConnectThroughHome("platformId", "typeName", randomAesKey: true);
```

也可只使用 `RabbitHomeClient` 查询路由，再自行构造 `RabbitSocketServer` / `RabbitSocketClient`。

## 模块

| 命名空间 | 作用 |
| --- | --- |
| `JLGames.RabbitClient` | `RabbitClientManager`：查询、连接、会话加密、进度事件 |
| `JLGames.RabbitClient.Home` | Home URL/密钥配置、路由查询、查询结果 |
| `JLGames.RabbitClient.Server` | Socket 连接、收发、连接/消息事件 |
| `JLGames.RabbitClient.Server.Message` | 消息头、请求/响应读写 |
| `JLGames.RabbitClient.Server.MMO` | 实体、变量、Meta、MMO 事件 |

## API 文档

### 中文

- **[API 文档总览](RabbitClient-API/cn/README.md)**
- **[Home](RabbitClient-API/cn/JLGames.RabbitClient.Home.md)**
- **[Server](RabbitClient-API/cn/JLGames.RabbitClient.Server.md)**
- **[Message](RabbitClient-API/cn/JLGames.RabbitClient.Server.Message.md)**
- **[MMO](RabbitClient-API/cn/JLGames.RabbitClient.Server.MMO.md)**
- **[RabbitClient](RabbitClient-API/cn/JLGames.RabbitClient.md)**（管理器）

### English

- **[API overview](RabbitClient-API/en/README.md)**
- **[Home](RabbitClient-API/en/JLGames.RabbitClient.Home.md)**
- **[Server](RabbitClient-API/en/JLGames.RabbitClient.Server.md)**
- **[Message](RabbitClient-API/en/JLGames.RabbitClient.Server.Message.md)**
- **[MMO](RabbitClient-API/en/JLGames.RabbitClient.Server.MMO.md)**
- **[RabbitClient](RabbitClient-API/en/JLGames.RabbitClient.md)**

发版流程见 [Release 说明](.github/workflows/Release.md)。补写已发布说明见 [ReleaseNote 说明](.github/workflows/ReleaseNote.md)。各 tag 说明在 `notes/release/ReleaseNotes_<tag>.md`。根目录 `CHANGELOG.md` 为历史记录，不再追加。

## 构建与测试

```bash
# 需与 Infra-CSharp 位于同一父目录
dotnet build RabbitClient-CSharp.sln --configuration Release

# 全部测试（依赖本机 Rabbit-Home HTTP 9000 / Rabbit-Server）
dotnet test RabbitClient-Test/RabbitClient-Test.csproj

# 与 CI 相同：跳过依赖本机服务的用例
dotnet test RabbitClient-Test/RabbitClient-Test.csproj --filter "Category!=RunOnlyThis"
```

- **Debug**: `RabbitClient/bin/Debug/netstandard2.0/`
- **Release**: `RabbitClient/bin/Release/netstandard2.0/`

推送到 `master` 或向 `master` 开 Pull Request 时运行 CI（`.github/workflows/CI.yml`），Infra 版本按 `Require.yml` 解析。手动运行 CI 时可填写本仓库 tag；留空则使用 `Require.yml` 最后一项。

## 发版

在 `master` 上推送符合 `v*.*.*` 的 tag 且该提交位于 `master` 时，Release 工作流会：

1. 在 `Require.yml` 中查找 `Tag` 等于该 tag 的项
2. 用该项的 `Infra-CSharp` 检出依赖
3. 构建 Debug / Release 两份 `netstandard2.0` DLL zip 并创建 GitHub Release

`Require.yml` 示例：

```yaml
Require:
  - Tag: v1.2.1
    Infra-CSharp: v1.4.1
```

## 许可证

本项目遵循 [MIT](LICENSE) 许可证。
