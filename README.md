# RabbitClient-CSharp

[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![C#](https://img.shields.io/badge/C%23-7.3+-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![CI](https://github.com/xuzhuoxi/Rabbit-Client-CSharp/actions/workflows/CI.yml/badge.svg)](https://github.com/xuzhuoxi/Rabbit-Client-CSharp/actions/workflows/CI.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

简体中文 | [English](README_EN.md)

## 项目简介
RabbitClient-CSharp 是一个用于访问 Rabbit-Home 服务器并自动选择、连接 Rabbit-Server 实例的 C# 客户端库。该库适用于需要动态发现和连接后端服务的分布式应用场景。

依赖旁路仓库 [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp)：本地需与本仓库位于同一父目录（`JLGameStudios/Infra-CSharp` 与 `JLGameStudios/RabbitClient-CSharp`）。当前以 GitHub Release 的 Debug/Release DLL 压缩包分发；NuGet 打包尚未启用。

## 主要功能
- 通过 Rabbit-Home 查询可用的 Rabbit-Server 实例。
- 支持密钥加密、Base64 编码等安全机制。
- 提供与 Rabbit-Server 的 Socket 通信能力。
- 统一的客户端管理器，简化连接与消息收发流程。
- 丰富的事件机制，便于集成与扩展。

## 目录结构
```
RabbitClient-CSharp/
├── RabbitClient/                 # 主库（netstandard2.0）
│   └── JLGames/RabbitClient/
│       ├── Home/                 # Rabbit-Home
│       ├── Server/               # Rabbit-Server、消息、MMO
│       └── RabbitClientManager.cs
├── RabbitClient-Test/            # 测试（net8.0）
├── RabbitClient-API/             # 生成的 API 文档
├── notes/release/                # 发行说明模板与各 tag 说明
└── RabbitClient-CSharp.sln
```

旁路依赖：`../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`

## 快速开始
1. 将本仓库与 Infra-CSharp 放在同一父目录下，或从 GitHub Release 下载对应 tag 的 zip 并引用其中的 `RabbitClient.dll`（包内同时包含 `Infra-CSharp.dll`）。
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
- **[API 文档总览](RabbitClient-API/cn/README.md)** - 完整的中文API文档索引
- **[Home 模块](RabbitClient-API/cn/JLGames.RabbitClient.Home.md)** - Rabbit-Home 服务器相关API
- **[Server 核心模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.md)** - Socket通信核心API
- **[消息协议模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.Message.md)** - 消息读写与协议API
- **[MMO模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.MMO.md)** - MMO游戏相关API

### 英文文档
- **[API Documentation Overview](RabbitClient-API/en/README.md)** - Complete English API documentation index
- **[Home Module](RabbitClient-API/en/JLGames.RabbitClient.Home.md)** - Rabbit-Home server related APIs
- **[Server Core Module](RabbitClient-API/en/JLGames.RabbitClient.Server.md)** - Socket communication core APIs
- **[Message Protocol Module](RabbitClient-API/en/JLGames.RabbitClient.Server.Message.md)** - Message read/write and protocol APIs
- **[MMO Module](RabbitClient-API/en/JLGames.RabbitClient.Server.MMO.md)** - MMO game related APIs

发版流程见 [Release 说明](.github/workflows/Release.md)。补写已发布说明见 [ReleaseNote 说明](.github/workflows/ReleaseNote.md)。

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

推送到 `master` 或向 `master` 开 Pull Request 时会运行 CI（`.github/workflows/CI.yml`）。打 `v*.*.*` tag 且提交在 `master` 上时，会构建并上传 Release 附件。

## 适用场景
- 游戏服务器动态发现与连接
- 分布式服务注册与发现
- 需要安全通信的客户端-服务端架构

## 许可证
本项目遵循 MIT 许可证。 