# 命名空间：JLGames.RabbitClient.Server.MMO

MMO 实体、变量、管理、事件、Meta 与整数向量。公开类型均在该命名空间下（源码按 Entity / Event / Lang / Meta / Var 目录组织）。

## 实体接口

### IEntity

继承 `IEquatable<IEntity>`。

- `EntityType`：实体类型
- `EntityId`：实体 Id

### IEntityPlayer

继承 `IEntity`、`IEquatable<IEntityPlayer>`、`IEventDispatcher`、`IVarSupport`、`IPosSupport`、`ITowardSupport`、`IInputSupport`、`IUpdateSupport`。

- `PlayerId`、`IsSelf`、`NickName`、`TeamId`
- `SetSelfPlayerId(string selfId)`

### IEntityRoom

继承 `IEntity`、`IEquatable<IEntityRoom>`、`IEventDispatcher`、`IUpdateSupport`、`IVarSupport`。

- `RoomId`、`RoomName`、`PlayerCount`、`UnitCount`
- `FindIPlayer(string playerId)` / `FindIUnit(string unitId)`
- `ForEachPlayer(Action<int, IEntityPlayer> each)` / `ForEachUnit(Action<int, IEntityUnit> each)`

### IEntityUnit

继承 `IEntity`、`IEquatable<IEntityUnit>`、`IEventDispatcher`、`IVarSupport`、`IPosSupport`、`ITowardSupport`、`IInputSupport`、`IUpdateSupport`。

- `UnitId`、`Owner`、`RoomId`

### 能力接口

**IVarSupport**

- `VarSet`
- `SetVar(string key, object value)` / `SetVars(IVarSet vars)`
- `DelVar(string key)` / `DelVars(string[] keys)`

**IPosSupport**

- `PosInt`
- `SetPosInt(V3Int xyz)` / `SetPosInt(int x, int y, int z)`

**ITowardSupport**

- `Toward`
- `SetTowardAngleInt(int towardAngleInt)` / `SetTowardAngleInt(short towardAngleInt)`

**IInputSupport**

- `InputMoveOn`、`InputTargetOn`、`InputMoveInt`、`InputTargetInt`
- `SetInputMoveInt(V3Int)` / `SetInputMoveInt(int x, int y, int z)`
- `SetInputTargetInt(V3Int)` / `SetInputTargetInt(int x, int y, int z)`

**IUpdateSupport**

- `UpdateFromReader(IRabbitResponseMsg reader)`

## 实体实现

### EntityPlayer

实现 `IEntityPlayer`，继承 `EventDispatcher`。

- 构造函数：`EntityPlayer(string playerId)`
- 静态：`GenPlayerFromReader(IRabbitResponseMsg reader)`、`NewPlayer()`、`NewIPlayer()`

### EntityRoom

实现 `IEntityRoom`，继承 `EventDispatcher`。

- 构造函数：`EntityRoom(string roomId)`
- `SetRoomId(string roomId)`
- `FindPlayer` / `RemovePlayer` / `AddPlayer` / `AddPlayers(int addCount)`
- `FindUnit` / `RemoveUnit` / `AddUnit` / `AddUnits(int addCount)`
- 静态：`GenRoomFromReader(IRabbitResponseMsg reader)`、`NewRoom()`、`NewIRoom()`

### EntityUnit

实现 `IEntityUnit`，继承 `EventDispatcher`。

- 构造函数：`EntityUnit(string unitId)`
- 静态：`GenUnitFromReader(IRabbitResponseMsg reader)`、`NewUnit()`、`NewIUnit()`

## 变量

### IVarSet

继承 `INetMessage`。

- `Size`：全部变量数量（含时间戳键）
- `KeySize`：业务键数量
- `KeyToStampKey(string key)`
- `Clear()`
- `SetVar(string key, object value)` / `SetVar(string key, object value, long timestamp)`
- `DeleteVar(string key, bool includeStampKey)`
- `SetVars(Dictionary<string, object> vars)` 及带时间戳 / `IVarSet` 重载
- `DeleteVars(string[] keys, bool includeStampKey)`
- `CheckKey(string key)`、`GetValue(string key)`、`GetValue<T>(string key)`、`GetValueStamp(string key)`
- `ForEach(VarSetDelegates.FuncEach)` / `ForEach(VarSetDelegates.FuncStampEach)`

值类型支持基础类型及其数组，以及 `V2Int` / `V3Int`。

### VarSet

实现 `IVarSet`。

- 构造函数：`VarSet(bool littleEndian)`
- 另实现 `EncodeToBytes()` / `DecodeFromBytes(byte[])`、`EncodeToBuff` / `DecodeFromBuff`

### VarData&lt;T&gt;

带时间戳的类型化变量项：`Key`、`Type`（`VarType`）、`Value`（赋值时更新 Stamp）、`Stamp`。

### VarSetDelegates

- `FuncEach(string key, object value)`
- `FuncStampEach(string key, object value, long stamp)`

## MmoManager

继承 `EventDispatcher`。绑定外部分发器以处理房间状态。

- `StartManager(IEventDispatcher dispatcher)`
- `StopManager()`

## 事件

### PlayerEvents

| 事件 | 载荷 |
| --- | --- |
| `EventLeaveRoom` | `NotifyPlayerLeaveRoomData`（`RoomId`、`PlayerId`、`Player`） |
| `EventEnterRoom` | `NotifyPlayerEnterRoomData`（`RoomId`、`Player`） |
| `NotifyPlayerVars` | `NotifyPlayerVarsData`（`PlayerId`、`VarSet`） |
| `NotifyPlayerDelVars` | 玩家变量删除 |
| `NotifyPlayerVarPos` | `NotifyPlayerVarsData` |

另有 `NotifyPlayerDelData`（`RoomId`、`Player`）。

### RoomEvents

| 事件 | 载荷 |
| --- | --- |
| `EventRoomEnter` | `roomId`（`string`） |
| `EventRoomExit` | `RoomReadyData`（`OldRoomId`、`NewRoomId`、`NewRoom`） |
| `NotifyRoomVar` | `NotifyRoomVarData`（`RoomId`、`VarSet`） |
| `NotifyRoomVarDel` | `NotifyRoomVarDelData`（`RoomId`、`Keys`） |

### UnitEvents

| 事件 | 载荷 |
| --- | --- |
| `EventUnitNew` | `EventUnitNewData`（`RsCode`、`Units`） |
| `NotifyUnitVars` | `NotifyUnitVarsData`（`UnitId`、`VarSet`） |
| `NotifyUnitDelVars` | 单位变量删除 |
| `NotifyUnitVarPos` | `NotifyUnitVarsData` |
| `NotifyUnitNew` | `NotifyUnitNewData`（`RoomId`、`PlayerId`、`Unit`） |
| `NotifyUnitDel` | `NotifyUnitDelData`（`RoomId`、`Unit`） |

### WorldEvents

- `EventWorldInit`：载荷为 `roomId`（`string`）

## Meta 与常量

### ProtoMMOCode

MMO 协议结果码（常量名与语义以源码 XML 注释为准）：

- `MMORoomExist`（-101，房间不存在）、`MMORoomNotExist`（-102，房间已存在）、`MMORoomCapLimit`（-103）
- `MMOTeamCorpsExist`（-104）、`MMOTeamCorpsNotExist`（-105）、`MMOTeamCorpsCapLimit`（-106）
- `MMOTeamExist`（-107）、`MMOTeamNotExist`（-108）、`MMOTeamCapLimit`（-109）
- `MMOChanExist`（-110）、`MMOChanNotExist`（-111）、`MMOChanCapLimit`（-112）
- `MMOPlayerExist`（-113）、`MMOPlayerNotExist`（-114）、`MMOPlayerInRoom`（-115）
- `MMOUnitExist`（-116）、`MMOUnitNotExist`（-117）
- `MMOIndexType`（-200）、`MMOOther`（-201）

### PlayerVarKeys

`Pos`、`Toward`、`InputMove`、`InputTarget`、`InputJump`、`ActionState`、`Hp`、`Buff`、`Nick`、`Team`、`TeamCorps`

### RoomVarKeys

- `Name`：房间名称（string）

### UnitVarKeys

`Owner`、`Room`、`Pos`、`Toward`、`InputMove`、`InputTarget`、`InputJump`、`ActionState`

### MmoMetas

- `GetRoomVarMeta` / `GetPlayerVarMeta` / `GetUnitVarMeta`
- `RegisterRoomVarMeta` / `RegisterPlayerVarMeta` / `RegisterUnitVarMeta`
- `MetaData`：`Key`、`Type`、`Default`

## 枚举

### EntityType（Flags）

`EntityUnit`、`EntityPlayer`、`EntityRoom`、`EntityTeam`、`EntityTeamCorps`、`EntityChannel`

**EntityTypeUtil**

- `EntityNone`、`EntityAll`
- `Match(this EntityType self, EntityType check)`：任意位重叠
- `Include(this EntityType self, EntityType check)`：包含 check 的全部位

### VarType

`Undefined`、`Moment`、`Forever`、`Duration`

### CampType

`None`、`Watch`、`Neutral`、`Camp1`～`Camp8`

### RoomType

`None`、`Normal`、`Temp`

### UnitType

`None`、`Building`、`Troop`、`Banner`

## 数学结构体

### V2Int

二维整型向量，实现 `IEquatable<V2Int>`、`IFormattable`。

- 分量：`x`、`y`；索引器 `this[int index]`
- 构造：`V2Int(int x, int y)`；`Set(int newX, int newY)`
- 静态：`MagnitudeLevel` / `MagnitudeValue`；`zero`、`one`、`up`、`down`、`left`、`right`
- `Min` / `Max` / `Lerp` / `LerpUnclamped`
- 运算符：`+`、`-`、一元 `-`、`*`、`/`、`==`、`!=`

### V3Int

三维整型向量，实现 `IEquatable<V3Int>`、`IFormattable`。

- 分量：`x`、`y`、`z`；索引器 `this[int index]`
- 构造：`V3Int(int x, int y, int z)`、`V3Int(int x, int y)`；`Set(int newX, int newY, int newZ)`
- 静态：`MagnitudeLevel` / `MagnitudeValue`；`zero`、`one`、`forward`、`back`、`up`、`down`、`left`、`right`
- `Min` / `Max` / `Lerp` / `LerpUnclamped`
- 运算符：`+`、`-`、一元 `-`、`*`、`/`、`==`、`!=`
