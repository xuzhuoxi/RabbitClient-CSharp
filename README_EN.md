# RabbitClient-CSharp

[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![C#](https://img.shields.io/badge/C%23-7.3+-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![CI](https://github.com/xuzhuoxi/RabbitClient-CSharp/actions/workflows/CI.yml/badge.svg)](https://github.com/xuzhuoxi/RabbitClient-CSharp/actions/workflows/CI.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

[简体中文](README.md) | English

A .NET Standard 2.0 client library: discover Rabbit-Server instances via Rabbit-Home, then connect over sockets and exchange encrypted messages. It depends on the sibling repo [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp).

## Table of contents

- [Overview](#overview)
- [Notes](#notes)
- [Features](#features)
- [Technology stack](#technology-stack)
- [Layout](#layout)
- [Installation](#installation)
- [Quick start](#quick-start)
- [Modules](#modules)
- [API docs](#api-docs)
- [Building and testing](#building-and-testing)
- [Releasing](#releasing)
- [License](#license)

## Overview

RabbitClient-CSharp is for clients that must discover backends at runtime: query Rabbit-Home for a route, then connect to that Rabbit-Server. The entry point is `JLGames.RabbitClient.RabbitClientManager`, which coordinates Home queries, socket I/O, AES session encryption, and event dispatch.

The library targets **.NET Standard 2.0** (C# 7.3) and runs on Windows, macOS, and Linux. Tests target **net8.0**.

Releases are Debug / Release DLL zips on GitHub; NuGet packing is not enabled yet.

## Notes

- **Pin Infra-CSharp to the mapped tag.** For source builds, keep this repo next to [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp) under the same parent (`JLGameStudios/Infra-CSharp` and `JLGameStudios/RabbitClient-CSharp`). CI / Release do not always check out Infra `master`. They read root `Require.yml`, find the `Require` item whose `Tag` equals this repo’s tag, and check out that item’s `Infra-CSharp` tag. Missing entries abort the workflow.
- **Use a matching Rabbit-Home / Rabbit-Server version.** Route queries, framing, and session keys evolve with the protocol; mixing versions can fail handshake or parsing.
- Tests that need a local Rabbit-Home (default HTTP `127.0.0.1:9000`) and Rabbit-Server are marked `RunOnlyThis` and are skipped in CI.
- Public API is defined by the source. Socket callbacks use `SynchronizationContext`.

## Features

- **Home discovery**: `RabbitHomeClient` queries Rabbit-Home for an available server (GET/POST; optional RSA encryption of query params).
- **Manager**: `RabbitClientManager.ConnectThroughHome` queries, connects, applies the AES session key, and starts receiving.
- **Sockets**: `RabbitSocketServer` / `RabbitSocketClient` for connect, send/receive, and `GetExtensionDispatcher` by Extension name.
- **Messages**: request/response types under `JLGames.RabbitClient.Server.Message` (`RabbitRequestMsg` / `RabbitResponseMsg`).
- **Session crypto**: AES from Home’s `OpenSk`; optional RSA on the query.
- **Events**: Home/Server progress, connect finished, send/receive, MMO room/player/unit events.
- **MMO**: Player / Room / Unit entities, `IVarSet`, Meta, and events.
- **Threading**: `SetThreadSocketContext(SynchronizationContext)` to marshal socket callbacks.

## Technology stack

- **.NET Standard 2.0** / **C# 7.3+** (tests: C# 10 / net8.0)
- **NUnit 3**
- **Infra-CSharp** as a sibling `ProjectReference` (net, events, crypto). No other required NuGet packages on the library
- **dotnet CLI**

## Layout

```
RabbitClient-CSharp/
├── RabbitClient/                 # Library (netstandard2.0)
│   └── JLGames/RabbitClient/
│       ├── Home/                 # Rabbit-Home
│       ├── Server/               # Sockets, messages, MMO
│       ├── RabbitClientManager.cs
│       └── RabbitClientManagerEvents.cs
├── RabbitClient-Test/            # Tests (net8.0)
├── RabbitClient-API/             # Bilingual API docs
├── notes/release/                # Release-note template and per-tag notes
├── Require.yml                   # This repo’s tag → Infra-CSharp tag
├── .github/workflows/            # CI / Release / ReleaseNote
└── RabbitClient-CSharp.sln
```

The solution also references `../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`.

## Installation

### Prerequisites

- A runtime compatible with .NET Standard 2.0
- .NET 8 SDK to build and test from source; local Rabbit-Home / Rabbit-Server for integration tests

### DLL from GitHub Release

From [GitHub Releases](https://github.com/xuzhuoxi/RabbitClient-CSharp/releases), download the zip for the target tag (`RabbitClient-CSharp_<tag>_release_netstandard2.0.zip` or `..._debug_...`) and reference `RabbitClient.dll`. The archive also includes `Infra-CSharp.dll`, `.pdb`, `.deps.json`, and license/readme files.

### Build from source

```bash
git clone https://github.com/xuzhuoxi/RabbitClient-CSharp.git
git clone https://github.com/xuzhuoxi/Infra-CSharp.git
# Both repos must sit under the same parent directory
cd RabbitClient-CSharp
dotnet build RabbitClient-CSharp.sln --configuration Release
```

Check out the Infra tag that `Require.yml` maps for this repo’s version. For example, this repo’s `v1.2.1` maps to Infra `v1.4.1`.

## Quick start

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
        // manager.SocketClient.SendMessage(...)
        // manager.SocketClient.GetExtensionDispatcher(name)
    }
});

await manager.ConnectThroughHome("platformId", "typeName", randomAesKey: true);
```

You can also use `RabbitHomeClient` alone, then construct `RabbitSocketServer` / `RabbitSocketClient` yourself.

## Modules

| Namespace | Role |
| --- | --- |
| `JLGames.RabbitClient` | `RabbitClientManager`: query, connect, session crypto, progress events |
| `JLGames.RabbitClient.Home` | Home URL/key settings, route query, results |
| `JLGames.RabbitClient.Server` | Socket connect/send/receive and connection events |
| `JLGames.RabbitClient.Server.Message` | Headers, request/response readers and writers |
| `JLGames.RabbitClient.Server.MMO` | Entities, vars, Meta, MMO events |

## API docs

### Chinese

- **[Overview](RabbitClient-API/cn/README.md)**
- **[Home](RabbitClient-API/cn/JLGames.RabbitClient.Home.md)**
- **[Server](RabbitClient-API/cn/JLGames.RabbitClient.Server.md)**
- **[Message](RabbitClient-API/cn/JLGames.RabbitClient.Server.Message.md)**
- **[MMO](RabbitClient-API/cn/JLGames.RabbitClient.Server.MMO.md)**
- **[RabbitClient](RabbitClient-API/cn/JLGames.RabbitClient.md)** (manager)

### English

- **[Overview](RabbitClient-API/en/README.md)**
- **[Home](RabbitClient-API/en/JLGames.RabbitClient.Home.md)**
- **[Server](RabbitClient-API/en/JLGames.RabbitClient.Server.md)**
- **[Message](RabbitClient-API/en/JLGames.RabbitClient.Server.Message.md)**
- **[MMO](RabbitClient-API/en/JLGames.RabbitClient.Server.MMO.md)**
- **[RabbitClient](RabbitClient-API/en/JLGames.RabbitClient.md)**

Release tagging is documented in [Release.md](.github/workflows/Release.md). Updating notes on an existing release is documented in [ReleaseNote.md](.github/workflows/ReleaseNote.md). Per-tag notes live at `notes/release/ReleaseNotes_<tag>.md`. Root `CHANGELOG.md` is historical and no longer appended.

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

Pushes and pull requests to `master` run CI (`.github/workflows/CI.yml`). Infra is resolved from `Require.yml`. Manual CI runs may pass this repo’s tag; if omitted, the last `Require.yml` item is used.

## Releasing

Pushing a `v*.*.*` tag whose commit is on `master` runs Release, which:

1. Looks up that tag in `Require.yml`
2. Checks out the mapped Infra-CSharp tag
3. Builds Debug / Release `netstandard2.0` DLL zips and creates a GitHub Release

`Require.yml` example:

```yaml
Require:
  - Tag: v1.2.1
    Infra-CSharp: v1.4.1
```

## License

This project is licensed under the [MIT](LICENSE) License.
