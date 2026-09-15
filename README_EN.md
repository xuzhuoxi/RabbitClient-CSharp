# RabbitClient-CSharp

[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![C#](https://img.shields.io/badge/C%23-7.3+-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![CI](https://github.com/xuzhuoxi/Rabbit-Client-CSharp/actions/workflows/CI.yml/badge.svg)](https://github.com/xuzhuoxi/Rabbit-Client-CSharp/actions/workflows/CI.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

[简体中文](README.md) | English

## Project Overview
RabbitClient-CSharp is a C# client library for accessing Rabbit-Home servers and automatically selecting and connecting to Rabbit-Server instances. This library is suitable for distributed application scenarios that require dynamic discovery and connection to backend services.

It depends on the sibling repo [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp): keep both under the same parent directory (`JLGameStudios/Infra-CSharp` and `JLGameStudios/RabbitClient-CSharp`). Releases are Debug/Release DLL zips on GitHub; NuGet packing is not enabled yet.

## Key Features
- Query available Rabbit-Server instances through Rabbit-Home.
- Support for security mechanisms such as key encryption and Base64 encoding.
- Provide Socket communication capabilities with Rabbit-Server.
- Unified client manager that simplifies connection and message sending/receiving processes.
- Rich event mechanisms for easy integration and extension.

## Directory Structure
```
RabbitClient-CSharp/
├── RabbitClient/                 # Library (netstandard2.0)
│   └── JLGames/RabbitClient/
│       ├── Home/                 # Rabbit-Home
│       ├── Server/               # Rabbit-Server, messages, MMO
│       └── RabbitClientManager.cs
├── RabbitClient-Test/            # Tests (net8.0)
├── RabbitClient-API/             # Generated API docs
├── notes/release/                # Release note template and per-tag notes
└── RabbitClient-CSharp.sln
```

Sibling dependency: `../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`

## Quick Start
1. Place this repo next to Infra-CSharp under the same parent directory, or download the GitHub Release zip for the target tag and reference `RabbitClient.dll` (the archive also includes `Infra-CSharp.dll`).
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
- **[API 文档总览](RabbitClient-API/cn/README.md)** - 完整的中文API文档索引
- **[Home 模块](RabbitClient-API/cn/JLGames.RabbitClient.Home.md)** - Rabbit-Home 服务器相关API
- **[Server 核心模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.md)** - Socket通信核心API
- **[消息协议模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.Message.md)** - 消息读写与协议API
- **[MMO模块](RabbitClient-API/cn/JLGames.RabbitClient.Server.MMO.md)** - MMO游戏相关API

### English Documentation
- **[API Documentation Overview](RabbitClient-API/en/README.md)** - Complete English API documentation index
- **[Home Module](RabbitClient-API/en/JLGames.RabbitClient.Home.md)** - Rabbit-Home server related APIs
- **[Server Core Module](RabbitClient-API/en/JLGames.RabbitClient.Server.md)** - Socket communication core APIs
- **[Message Protocol Module](RabbitClient-API/en/JLGames.RabbitClient.Server.Message.md)** - Message read/write and protocol APIs
- **[MMO Module](RabbitClient-API/en/JLGames.RabbitClient.Server.MMO.md)** - MMO game related APIs

Release tagging is documented in [Release.md](.github/workflows/Release.md). Updating notes on an existing release is documented in [ReleaseNote.md](.github/workflows/ReleaseNote.md).

## Building and testing

```bash
# Infra-CSharp must sit next to this repo
dotnet build RabbitClient-CSharp.sln --configuration Release

# All tests (need local Rabbit-Home on HTTP 9000 / Rabbit-Server)
dotnet test RabbitClient-Test/RabbitClient-Test.csproj

# Same as CI: skip tests that need a local server
dotnet test RabbitClient-Test/RabbitClient-Test.csproj --filter "Category!=RunOnlyThis"
```

- **Debug**: `RabbitClient/bin/Debug/netstandard2.0/`
- **Release**: `RabbitClient/bin/Release/netstandard2.0/`

Pushes and pull requests to `master` run CI (`.github/workflows/CI.yml`). Pushing a `v*.*.*` tag whose commit is on `master` builds and uploads Release assets.

## Use Cases
- Dynamic discovery and connection of game servers
- Distributed service registration and discovery
- Client-server architectures requiring secure communication

## License
This project follows the MIT License.
