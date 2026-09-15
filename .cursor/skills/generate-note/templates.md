# ReleaseNote 模板

生成或补充 `notes/release/ReleaseNotes_{tag}.md` 前阅读本文件。节名与 `notes/release/ReleaseNotes.md` 一致；文风与现有 `notes/release/ReleaseNotes_v*.md` 一致。

结构：开头亮点总述，再分 Notable / Breaking / Fixes，最后是完整 Changelog、New Contributors 与 Full Changelog 对照（对照写成本地 git 范围，不要写成远程链接）。

本仓库面向 Rabbit-Home / Rabbit-Server 客户端（`JLGames.RabbitClient`：Home、Server、Message、MMO）。归纳时按这些模块归类条目，但 **不要** 改用根目录 `CHANGELOG.md` 那种自定义小节名。

## 新文件结构

```markdown
## Release Notes

+ 一两句总述（自上一版本到本 tag 的要点）。

### Known Issues

### Notable Changes

### Improvements

### Breaking Changes

### API Changes

### Changes

### Notable Fixes

### Fixes

### Changelog

### New Contributors

**Full Changelog**: {RANGE_START}...{RANGE_END}

## Library Changes

### library Updated

#### Updated

#### No Longer Available

#### Added
```

## 填写规则

- 不要写 `# RabbitClient-CSharp` 大标题，文档从 `## Release Notes` 起头
- 标题用模板英文；条目用中文，`+ ` 开头
- 提交说明常中英并列：有中文段只用中文，不要中英各写一条
- 模板中部分条目没有内容时，生成的文档中可以不包含该条目：无条目的 `###` / `####` 整节不要写出；`## Library Changes` 下没有任何库变更时整节不要写出。不要留空标题
- 总述：写本版本最重要的一两件事，不要把下面条目再抄一遍
- **Notable Changes**：最值得关注的能力或行为（亮点列表，不必覆盖全部提交）。优先写 Home 发现 / 连接、Socket 收发、消息协议、MMO 实体与事件、`RabbitClientManager` 等对外能力
- **Improvements**：其余新能力、测试、API 文档（`RabbitClient-API`）、注释、文档完善
- **Breaking Changes**：不兼容变更（升级语言 / 目标框架版本、改签名、删除 API、改事件名或协议字段等）
- **API Changes**：公开 API 增删改（若已写入 Breaking Changes 且无更多条目，可只保留一处）
- **Changes**：对外行为、配置、默认策略等非修复变更
- **Notable Fixes**：重要缺陷修复（不必列出全部）；跨线程、消息收发异常、事件类型（如 ProtoId / ProtoUid）这类优先放这里
- **Fixes**：其余修复；与 Notable Fixes 重复的不要再写一遍
- **Changelog**：范围内全部提交的完整列表（主题 + 短哈希），一条提交只列一次，用中文主题
- **New Contributors**：该范围内首次出现的提交者；无则省略
- **Full Changelog**：上一本地 tag 与范围终点的对照，只写成 `{RANGE_START}...{RANGE_END}`（本地 git 范围）。`RANGE_END` 为已存在的本地指定 tag，或当前本地 `HEAD`。仓库无本地 tag、或没有上一档 tag 时省略该行。不要写成 URL
- `Library Changes`：只写范围内 `*.csproj`（及 `Directory.Packages.props`，若有）里实际变化的依赖
  - NuGet：`PackageReference` 写成 `包名：旧 → 新`
  - 旁路项目：`RabbitClient.csproj` / `RabbitClient-Test.csproj` 对 `Infra-CSharp` 的 `ProjectReference` 仅在路径或引用本身变化时记录；不要把 `../Infra-CSharp` 仓库里的提交当成本库变更
- `### library Updated` 中的 `library` 换成真实包名，或不用该占位、直接在 `## Library Changes` 下写 `+ 包名：旧 → 新`
- **Known Issues**：明确未解决的限制；无则省略。与 Go 侧 Rabbit 服务互通时若存在版本对齐要求，写在这里

## 不要做

- 不要输出到 `CHANGELOG.md`，也不要按它的 `### Rabbit-Server` 结构写
- 不要编造 NuGet 版本或 Infra-CSharp 的 tag
- 不要把测试工程里的 NUnit / 覆盖率包变化写成客户端库的 Notable Changes（放到 Library Changes 或 Improvements 即可）
- 不要写远程对照链接，不要根据远程引用归纳变更
