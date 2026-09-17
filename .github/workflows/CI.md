# CI 说明

本文说明 `.github/workflows/CI.yml` 的触发条件、Infra 版本如何解析、工作流内部流程，以及常见失败。

发版请使用 `Release.yml`（见同目录 [Release.md](Release.md)）。

## 1. 做什么

在推送到 `master`、向 `master` 开 Pull Request，或在 Actions 里手动运行时，自动：

1. 将本仓库与旁路依赖 [Infra-CSharp](https://github.com/xuzhuoxi/Infra-CSharp) 检出为兄弟目录（与本地 `JLGameStudios/Infra-CSharp`、`JLGameStudios/RabbitClient-CSharp` 布局一致）
2. 读取根目录 `Require.yml` 的 `Default.Infra-CSharp`，决定检出哪一版 Infra
3. 以 Release 配置构建解决方案
4. 运行单元测试（跳过依赖本机服务的用例）并收集覆盖率
5. 若有 cobertura 报告，上传到 Codecov

不创建 GitHub Release，不打包 DLL zip。本仓库与 Infra 均按公开仓库处理：`actions/checkout` 用默认 `GITHUB_TOKEN` 检出旁路依赖。本仓库若仍为私有，拉 Infra 会 404，需先把本仓库设为公开。

可调整常量在 `CI.yml` 顶部的 `env`：`INFRA_REPO`（旁路依赖仓库，默认 `xuzhuoxi/Infra-CSharp`）。

本库通过 `ProjectReference` 引用 `../../Infra-CSharp/Infra-CSharp/Infra-CSharp.csproj`，因此两个仓库必须作为 `github.workspace` 下的兄弟目录检出。

## 2. 触发条件

```yaml
on:
  push:
    branches:
      - master
  pull_request:
    branches:
      - master
  workflow_dispatch:
```

| 事件 | 何时运行 |
| --- | --- |
| `push` 到 `master` | 合入或直接推送到 `master` |
| `pull_request` 目标为 `master` | 打开、更新或同步 PR |
| `workflow_dispatch` | Actions 网页上手动 **Run workflow**，无输入项 |

不响应 tag 推送。打 `v*.*.*` tag 走的是 Release 工作流。

GitHub 使用的是**触发那次提交**上的 `CI.yml` 与 `Require.yml`。改工作流或默认 Infra 版本后，须先合入将要跑 CI 的分支。

## 3. 运行前检查

- 仓库已启用 Actions。
- 根目录存在有效的 `Require.yml`，且含 `Default.Infra-CSharp`（见第 4 节）。
- 本仓库为公开（否则默认 `GITHUB_TOKEN` 无法拉 Infra）。`xuzhuoxi/Infra-CSharp` 可公开检出；若 `Default` 指向分支、tag 或短哈希，该 ref 必须在 Infra 仓库中存在。
- 测试项目依赖均为公开 NuGet 包，不需要额外 private token。

## 4. Infra 版本（`Require.yml` 的 `Default`）

CI **只读** `Default.Infra-CSharp`，**不读** `Require` 数组。`Require` 数组由 Release 按本仓库 tag 查找，见 [Release.md](Release.md)。

```yaml
Default:
  Infra-CSharp: v1.4.1
Require:
  - Tag: v1.2.1
    Infra-CSharp: v1.4.1
```

`Default.Infra-CSharp` 与 `Require[n].Infra-CSharp` 取值格式相同（先匹配 tag，再匹配短哈希，其余视为分支名）：

| 取值 | 检出内容 | 示例 |
| --- | --- | --- |
| 分支名 | Infra 该分支当前最新提交 | `master`、`main`、`develop` |
| `v*.*.*` | Infra 的 git tag | `v1.4.1`、`v1.4.1-rc.1` |
| 7–40 位十六进制 | Infra 的一次 git 提交（短哈希或完整 SHA） | `a1b2c3d` |

解析由 `.github/scripts/resolve-infra-ref.py Require.yml --default` 完成。字段缺失、值为空、或不属于上述三类时，该步骤失败并输出 `::error::`，工作流中止。分支名会先用 `git ls-remote` 确认 Infra 上存在该分支，再交给 `actions/checkout` 取该分支顶端。

写分支名时，同一份 `Require.yml` 在不同日期可能检到不同的 Infra 提交。需要对齐某一版时，改成具体 tag 或短哈希。

## 5. 手动运行

1. 打开 **Actions → CI → Run workflow**。
2. **Use workflow from** 选要构建的分支（一般为 `master` 或功能分支）。该选择同时决定读哪一次提交上的 `CI.yml` 和 `Require.yml`。
3. 没有 tag 输入框。要换 Infra 版本，先改该分支上的 `Default.Infra-CSharp` 再运行。

## 6. 工作流执行步骤

### Job：Build and test

| 步骤 | 行为 |
| --- | --- |
| checkout 本仓库 | 克隆到 `RabbitClient-CSharp/`（`fetch-depth: 0`） |
| Resolve Infra-CSharp ref | 安装 PyYAML，按 `Default.Infra-CSharp` 写出 `ref` |
| checkout Infra-CSharp | `actions/checkout` 按解析出的 `ref` 检出到兄弟目录 `Infra-CSharp/` |
| setup-dotnet | .NET 8 SDK（构建 `netstandard2.0` 主库与 `net8.0` 测试） |
| Run build | `dotnet build RabbitClient-CSharp.sln --configuration Release` |
| Run test with coverage | `dotnet test`，`--filter Category!=RunOnlyThis`，收集 cobertura |
| Record coverage presence | 若有 `coverage.cobertura.xml` 则 `has_coverage=true` |
| Upload coverage artifact | 仅当有覆盖率文件时上传 artifact `coverage` |

`RunOnlyThis` 用例依赖本机 Rabbit-Home（HTTP 9000）和 Rabbit-Server，CI 中跳过。本地要跑全部测试：

```bash
dotnet test RabbitClient-Test/RabbitClient-Test.csproj
```

### Job：Upload coverage

仅当 `has_coverage=true` 时运行：下载 artifact，用 `codecov/codecov-action` 上传。`fail_ci_if_error: false`，Codecov 失败不会把 CI 标红。

## 7. 注意事项

1. **改 `Default.Infra-CSharp` 后要推到会跑 CI 的分支。** 手动 Run workflow 读的是所选分支上的 `Require.yml`，不是你本机未推送的修改。
2. **CI 与 Release 用的 Infra 可以不同。** CI 看 `Default`；Release 看 `Require` 数组里与发版 tag 对应的项。发版前请确认 `Require` 中该项已写好。
3. **短哈希过短或 Infra 侧没有该提交**，`actions/checkout` 会失败。建议至少 7 位，且该提交对公开仓库可见。
4. **覆盖率不是硬性门槛。** 没有 cobertura 文件时跳过上传；Codecov 出错也不使 job 失败。
5. **本仓库需要是公开的。** 私有仓库的 `GITHUB_TOKEN` 不能拉其它仓库（含公开的 Infra），checkout 会 404。
6. **本工作流不发版。** 推 tag 请看 [Release.md](Release.md)。

## 8. 常见失败

| 现象 | 可能原因 |
| --- | --- |
| Resolve Infra-CSharp ref 失败 | 缺少 `Default` / `Default.Infra-CSharp`；值不是分支名、`v*.*.*` 或十六进制短哈希；或 Infra 上没有该分支 |
| checkout Infra-CSharp 报 `Repository not found` | 本仓库仍为私有；或 `INFRA_REPO` 写错 |
| checkout Infra-CSharp 找不到 ref | tag 或短哈希在 Infra 上不存在 |
| 找不到 Infra-CSharp.csproj | 旁路检出失败，或 csproj 的 `ProjectReference` 路径已改 |
| 构建失败 | Infra 版本与当前主库 API 不兼容（可把 `Default` 换成已知可用的 tag） |
| 测试超时 / hang | `--blame-hang-timeout` 为 5 分钟；个别用例卡住 |
| 没有覆盖率上传 | 测试未生成 `coverage.cobertura.xml`，属预期时忽略 |

## 9. 相关路径

| 路径 | 用途 |
| --- | --- |
| `.github/workflows/CI.yml` | CI 工作流 |
| `.github/workflows/CI.md` | 本文 |
| `.github/scripts/resolve-infra-ref.py` | 解析 `Require.yml`（CI 用 `--default`） |
| `Require.yml` | `Default` 供 CI；`Require` 数组供 Release |
| `RabbitClient-CSharp.sln` | 构建入口 |
| `RabbitClient-Test/RabbitClient-Test.csproj` | 测试项目 |
