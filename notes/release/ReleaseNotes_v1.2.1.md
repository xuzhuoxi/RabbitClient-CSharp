## Release Notes

+ 首个面向发行说明的版本：提供经 Rabbit-Home 发现并连接 Rabbit-Server 的 .NET Standard 2.0 客户端库（Home、Socket 收发、消息协议、MMO），并配套加密通信、线程上下文、CI / Release 工作流与中英 API 文档。

### Known Issues

+ NuGet 打包尚未启用，目前通过 GitHub Release 的 Debug/Release zip 分发 DLL。
+ 依赖本机 Rabbit-Home / Rabbit-Server 的用例标了 `RunOnlyThis`，CI 中会跳过。

### Notable Changes

+ 初始化 `JLGames.RabbitClient`：向 Rabbit-Home 查询可用服务器、连接 Rabbit-Server，以及经 Home 发现后再连接 Server。
+ 完成与 Rabbit-Home / Rabbit-Server 的连接及加密通信。
+ 增加 MMO 客户端支持：实体（Player / Room / Unit）、变量集、相关事件与 Meta（如 `PlayerVarKeys`、`ProtoMMOCode`、`RoomVarKeys`、`UnitVarKeys`）。
+ `RabbitSocketClient` 可按 Extension 名称取得对应 `IEventDispatcher`；发送前后分别抛出 `EventOnClientSendMessagePrepare`、`EventOnClientSendMessage`。
+ 接入 GitHub Actions：CI 构建测试、Release 打包、ReleaseNote 更新；CI / Release 从 `Require.yml` 读取本仓库 tag 对应的 Infra-CSharp 版本再检出依赖。
+ 补全 Home、Server、Message、MMO、`RabbitClientManager` 的接口注释，并提供中英 API 文档与 README。

### Improvements

+ 补充 LICENSE、中英 API 文档（`RabbitClient-API`）与 README。
+ 完善 `RabbitSocketClient` / `RabbitSocketServer` / `RabbitClientManager` 在非常规调用路径上的异常处理与 `Dispose`。
+ 更新 `EventOnConnectFinish` 事件说明。
+ 子项目与仓库更名为 `RabbitClient` / `RabbitClient-CSharp`。
+ 增加 generate-note skill；为包含监听服务的单元测试添加 `RunOnlyThis`，以便 CI 跳过。

### Breaking Changes

+ `IRabbitMessage` 及消息相关类型命名空间改为 `JLGames.RabbitClient.Server.Message`。
+ 线程上下文统一为 `SynchronizationContext`；`RabbitClientManager`、`RabbitSocketServer` 中设置线程上下文的函数声明随之调整。
+ 各 Extension 抛出的事件类型由 `ProtoUid` 改为 `ProtoId`。

### API Changes

+ 新增 Home 查询与经 Home 连接 Server、以及直接连接 Rabbit-Server 的客户端 API。
+ 新增 MMO 实体、事件、Meta 与变量相关类型；后续更新了 MMO 事件及其事件数据。
+ `RabbitSocketClient` 增加按 Extension 名称获取 `IEventDispatcher`。
+ 新增发送准备 / 发送完成通知：`EventOnClientSendMessagePrepare`、`EventOnClientSendMessage`。
+ 收发消息捕获异常时抛出事件。
+ `SetThreadSocketContext(SynchronizationContext)` 用于绑定 Socket 线程上下文。

### Changes

+ Infra-CSharp 依赖由 DLL `HintPath` 改为旁路目录的 `ProjectReference`。
+ 新增 `Require.yml`，CI / Release 按其中 `Tag` → `Infra-CSharp` 映射检出依赖，不再固定 `master` 或同名 tag。

### Notable Fixes

+ 修复 `RabbitSocketServer` 未完全用 `FixedThreadContext` 处理异步结果导致的跨线程调用。
+ 修复各 Extension 事件实例误抛 `ProtoUid`、应为 `ProtoId` 的问题。

### Fixes

+ `RabbitSocketClient` / `RabbitSocketServer` / `RabbitClientManager` 补充非常规流程下的异常处理。

### Changelog

+ 初始化仓库：Home 查询、连接 Rabbit-Server、经 Home 连接 Server，并添加测试 (`a65bdbe`)
+ `IRabbitMessage` 命名空间改为 `JLGames.RabbitClient.Server.Message`，增加 MMO 基础代码 (`e721de0`)
+ 完成与 Rabbit-Home / Rabbit-Server 的连接及加密通信，并补齐功能测试 (`8eef506`)
+ 补充 LICENSE、中英 API 文档与 README (`9981966`)
+ 子项目更名为 Rabbit-Client (`c5eea1f`)
+ Socket 客户端/服务端与 `RabbitClientManager` 增加异常处理与 Dispose (`84bf055`)
+ 更新 `EventOnConnectFinish` 事件说明 (`ffbb8f9`)
+ 调整项目路径，依赖改为旁路 `ProjectReference` (`94dc189`)
+ 项目更名为 RabbitClient-CSharp (`c269ab9`)
+ `RabbitSocketClient` 增加按 Extension 名称获取 `IEventDispatcher` (`999eac6`)
+ 更新 MMO Meta：`PlayerVarKeys`、`ProtoMMOCode`、`RoomVarKeys`、`UnitVarKeys` (`ad14aa9`)
+ 修复 `RabbitSocketServer` 异步结果未走 `FixedThreadContext` 的跨线程问题 (`506107d`)
+ 线程上下文参数统一为 `SynchronizationContext` (`6fcfa07`)
+ 调整 `RabbitClientManager`、`RabbitSocketServer` 中线程上下文相关函数声明 (`f6ad1f4`)
+ 修复 Extension 事件应抛 `ProtoId` 而非 `ProtoUid` (`8bee77a`)
+ 更新 MMO 相关事件及其事件数据 (`4f9bdac`)
+ 收发消息捕获异常时抛出事件 (`88bef76`)
+ 发送前后抛出 `EventOnClientSendMessagePrepare` / `EventOnClientSendMessage` (`582da6d`)
+ 补全 Home 客户端模块注释 (`aa9c889`)
+ 补全 Server、Message、MMO、`RabbitClientManager` 注释 (`b1efec4`)
+ 增加 generate-note skill、CI / Release / ReleaseNote 工作流，测试添加 `RunOnlyThis` (`b7ed72e`)
+ 增加 `Require.yml`，CI / Release 按映射检出 Infra-CSharp (`d76d738`)

### New Contributors

+ xuzhuoxi

## Library Changes

+ 主库对 Infra-CSharp 由 `HintPath` 引用 DLL，改为 `ProjectReference`：`../../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`
+ 测试项目增加：`coverlet.collector` 6.0.0、`Microsoft.NET.Test.Sdk` 17.8.0、`NUnit` 3.14.0、`NUnit.Analyzers` 3.9.0、`NUnit3TestAdapter` 4.5.0
