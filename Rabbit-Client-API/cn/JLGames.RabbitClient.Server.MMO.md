# 命名空间：JLGames.RabbitClient.Server.MMO

本命名空间包含与 MMO 实体、变量、管理、事件、元数据、类型常量、数学结构体等相关的接口与实现。

## 接口与类

### IEntity
- 描述：MMO实体基础接口。
- 主要属性：
  - `EntityType`：实体类型（Entity type）。
  - `EntityId`：实体Id（Entity id）。

### IEntityPlayer
- 描述：玩家实体接口，继承自IEntity、IEquatable<IEntityPlayer>、IEventDispatcher、IVarSupport、IPosSupport、ITowardSupport、IInputSupport、IUpdateSupport。
- 主要属性：
  - `PlayerId`：玩家Id。
  - `IsSelf`：是否为当前玩家（Is it the current player）。
  - `NickName`：玩家昵称（Player nick name）。
  - `TeamId`：团队Id（Team Id）。
- 主要方法：
  - `SetSelfPlayerId(string selfId)`：设置当前玩家Id。

### IEntityRoom
- 描述：房间实体接口，继承自IEntity、IEquatable<IEntityRoom>、IEventDispatcher、IUpdateSupport、IVarSupport。
- 主要属性：
  - `RoomId`：房间Id。
  - `RoomName`：房间名称。
  - `PlayerCount`：玩家数量。
  - `UnitCount`：单位数量。
- 主要方法：
  - `FindIPlayer(string playerId)`：查找玩家信息。
  - `FindIUnit(string unitId)`：查找单位信息。
  - `ForEachPlayer(Action<int, IEntityPlayer> each)`：遍历每个玩家。
  - `ForEachUnit(Action<int, IEntityUnit> each)`：遍历每个单位。

### IEntityUnit
- 描述：单位实体接口，继承自IEntity、IEquatable<IEntityUnit>、IEventDispatcher、IVarSupport、IPosSupport、ITowardSupport、IInputSupport、IUpdateSupport。
- 主要属性：
  - `UnitId`：单位Id。
  - `Owner`：拥有者。
  - `RoomId`：所在房间Id。

### IVarSet
- 描述：变量集合接口，继承自INetMessage。
- 主要属性：
  - `Size`：全部变量数量（all var size）。
  - `KeySize`：变量数量（key var size）。
- 主要方法：
  - `KeyToStampKey(string key)`：变量键转变量时间戳键。
  - `Clear()`：清理变量集合。
  - `SetVar(string key, object value)`：设置变量值。
  - `SetVar(string key, object value, long timestamp)`：设置变量值（带时间戳）。
  - `DeleteVar(string key, bool includeStampKey)`：删除变量值，并返回。
  - `SetVars(Dictionary<string, object> vars)`：批量设置变量值。
  - `SetVars(Dictionary<string, object> vars, long timestamp)`：批量设置变量值（带时间戳）。
  - `SetVars(IVarSet set)`：批量设置变量值（来自另一个IVarSet）。
  - `SetVars(IVarSet set, long timestamp)`：批量设置变量值（带时间戳）。
  - `DeleteVars(string[] keys, bool includeStampKey)`：批量删除变量。
  - `CheckKey(string key)`：检查是否包含key。
  - `GetValue(string key)`：获取值。
  - `GetValue<T>(string key)`：获取泛型值。
  - `GetValueStamp(string key)`：获取值设置时的时间戳。
  - `ForEach(VarSetDelegates.FuncEach each)`：遍历变量。
  - `ForEach(VarSetDelegates.FuncStampEach stampEach)`：遍历变量（带时间戳）。

### IVarSupport
- 描述：变量支持接口。
- 主要属性：
  - `VarSet`：变量集合。
- 主要方法：
  - `SetVar(string key, object value)`：设置本地属性。
  - `SetVars(IVarSet vars)`：批量设置本地属性。
  - `DelVar(string key)`：删除本地属性。
  - `DelVars(string[] keys)`：批量删除本地属性。

### IPosSupport
- 描述：位置支持接口。
- 主要属性：
  - `PosInt`：位置。
- 主要方法：
  - `SetPosInt(V3Int xyz)`：设置坐标。
  - `SetPosInt(int x, int y, int z)`：设置坐标。

### ITowardSupport
- 描述：朝向支持接口。
- 主要属性：
  - `Toward`：朝向。
- 主要方法：
  - `SetTowardAngleInt(int towardAngleInt)`：设置目标朝向。
  - `SetTowardAngleInt(short towardAngleInt)`：设置目标朝向。

### IInputSupport
- 描述：输入支持接口。
- 主要属性：
  - `InputMoveOn`：有输入状态。
  - `InputTargetOn`：有目标。
  - `InputMoveInt`：控制。
  - `InputTargetInt`：目标位置。
- 主要方法：
  - `SetInputMoveInt(V3Int input)`：设置输入状态。
  - `SetInputMoveInt(int x, int y, int z)`：设置输入状态。
  - `SetInputTargetInt(V3Int input)`：设置目标位置。
  - `SetInputTargetInt(int x, int y, int z)`：设置目标位置。

### IUpdateSupport
- 描述：更新支持接口。
- 主要方法：
  - `UpdateFromReader(IRabbitResponseMsg reader)`：从一个IRabbitResponseMsg对象中更新数据。

### MmoManager
- 描述：MMO管理器，继承自EventDispatcher。
- 主要方法：
  - `StartManager(IEventDispatcher dispatcher)`：启动管理器。
  - `StopManager()`：停止管理器。

### VarSet
- 描述：变量集合实现，支持多种类型变量的存储、批量操作、序列化等。
- 主要方法：
  - 详见IVarSet接口定义。
  - `EncodeToBytes()`：编码为字节数组。
  - `DecodeFromBytes(byte[] bytes)`：从字节数组解码。
  - `EncodeToBuff(IDataBufferWriter buff)`：编码到缓冲区。
  - `DecodeFromBuff(IDataBufferReader buff)`：从缓冲区解码。

### EntityType（枚举）
- 描述：实体类型。
- 枚举值：
  - `EntityUnit`：单位实体。
  - `EntityPlayer`：玩家实体。
  - `EntityRoom`：房间实体。
  - `EntityTeam`：团队实体。
  - `EntityTeamCorps`：战队实体。
  - `EntityChannel`：频道实体。
- 相关静态工具：
  - `EntityTypeUtil.EntityNone`：无类型。
  - `EntityTypeUtil.EntityAll`：所有类型。
  - `EntityTypeUtil.Match(EntityType self, EntityType check)`：检查self是否为check中的一部分（check where 'self' is a part of 'check'）。
  - `EntityTypeUtil.Include(EntityType self, EntityType check)`：检查self是否包含check（Check if self contains check）。 

---

## 事件与事件数据结构

### PlayerEvents
- 事件常量：
  - `EventLeaveRoom`：玩家离开房间（事件数据：LeaveRoomData）
  - `EventEnterRoom`：玩家进入房间（事件数据：EnterRoomData）
  - `NotifyPlayerVar`：玩家变量变更（事件数据：NotifyPlayerVarData）
  - `NotifyPlayerVarDel`：玩家变量删除（事件数据：NotifyPlayerVarDelData）
  - `NotifyPlayerVarPos`：玩家坐标变更（事件数据：NotifyPlayerVarData）
- 事件数据结构：
  - `NotifyPlayerLeaveRoomData`：房间Id、玩家Id、IEntityPlayer
  - `NotifyPlayerEnterRoomData`：房间Id、IEntityPlayer
  - `NotifyPlayerVarData`：玩家Id、IVarSet
  - `NotifyPlayerVarDelData`：玩家Id、Keys

### RoomEvents
- 事件常量：
  - `EventRoomEnter`：房间初始化（数据：roomId）
  - `EventRoomExit`：房间退出（数据：RoomReadyData）
  - `NotifyRoomVar`：房间变量变更（数据：NotifyRoomVarData）
  - `NotifyRoomVarDel`：房间变量删除（数据：NotifyRoomVarDelData）
- 事件数据结构：
  - `NotifyRoomVarData`：RoomId、IVarSet
  - `NotifyRoomVarDelData`：RoomId、Keys
  - `RoomReadyData`：OldRoomId、NewRoomId、IEntityRoom

### UnitEvents
- 事件常量：
  - `EventUnitNew`：玩家创建新单位（数据：EventUnitNewData）
  - `NotifyUnitVar`：单位变量变更（数据：NotifyUnitVarData）
  - `NotifyUnitVarDel`：单位变量删除（数据：NotifyUnitVarDelData）
  - `NotifyUnitVarPos`：单位坐标变更（数据：NotifyUnitVarData）
  - `NotifyUnitNew`：新单位诞生（数据：NotifyNewUnitData）
  - `NotifyUnitDel`：单位被删除（数据：NotifyDelUnitData）
- 事件数据结构：
  - `EventUnitNewData`：RsCode、IEntityUnit[]
  - `NotifyUnitNewData`：RoomId、PlayerId、IEntityUnit
  - `NotifyUnitVarData`：UnitId、IVarSet
  - `NotifyUnitVarDelData`：UnitId、Keys
  - `NotifyUnitDelData`：RoomId、IEntityUnit

### WorldEvents
- 事件常量：
  - `EventWorldInit`：世界初始化（数据：roomId）

---

## 元数据与常量

### ProtoMMOCode
- MMO相关错误码常量：
  - `MMORoomExist`：房间不存在
  - `MMORoomNotExist`：房间已存在
  - `MMORoomCapLimit`：房间容量上限
  - `MMOTeamCorpsExist`：团队不存在
  - `MMOTeamCorpsNotExist`：团队已存在
  - `MMOTeamCorpsCapLimit`：团队容量上限
  - `MMOTeamExist`：队伍不存在
  - `MMOTeamNotExist`：队伍已存在
  - `MMOTeamCapLimit`：队伍容量上限
  - `MMOChanExist`：频道不存在
  - `MMOChanNotExist`：频道已存在
  - `MMOChanCapLimit`：频道容量上限
  - `MMOPlayerExist`：用户已存在
  - `MMOPlayerNotExist`：用户不存在
  - `MMOPlayerInRoom`：用户已在房间中
  - `MMOUnitExist`：单位已存在
  - `MMOUnitNotExist`：单位不存在
  - `MMOIndexType`：索引类型不匹配
  - `MMOOther`：其它错误

### PlayerVarKeys
- 玩家变量Key常量：
  - `Pos`：int32数组，坐标X、Y、Z
  - `Toward`：朝向，int16
  - `InputMove`：int32数组，输入X、Y、Z
  - `InputTarget`：int32数组，目标X、Y、Z
  - `InputJump`：输入状态Jump(bool)
  - `ActionState`：动作状态(uint32)
  - `Hp`：耐久(uint32)
  - `Buff`：Buff(uint32)
  - `Nick`：昵称(string)
  - `Team`：队伍id(string)
  - `TeamCorps`：军团Id(string)

### RoomVarKeys
- 房间变量Key常量：
  - `Name`：房间名称(string)

### UnitVarKeys
- 单位变量Key常量：
  - `Owner`：拥有者
  - `Room`：所有房间Id
  - `Pos`：int32数组，坐标X、Y、Z
  - `Toward`：朝向，int16
  - `InputMove`：int32数组，输入X、Y、Z
  - `InputTarget`：int32数组，目标X、Y、Z
  - `InputJump`：输入状态Jump(bool)
  - `ActionState`：动作状态(uint32)

### MmoMetas
- 静态类，提供房间、玩家、单位变量的元数据（MetaData：Key、Type、Default），支持注册和查询。
  - `GetRoomVarMeta(string key)`：获取房间变量元数据
  - `GetPlayerVarMeta(string key)`：获取玩家变量元数据
  - `GetUnitVarMeta(string key)`：获取单位变量元数据
  - `RegisterRoomVarMeta(MetaData metaData)`：注册房间变量元数据
  - `RegisterPlayerVarMeta(MetaData metaData)`：注册玩家变量元数据
  - `RegisterUnitVarMeta(MetaData metaData)`：注册单位变量元数据
- 内部类：
  - `MetaData`：
    - `Key`：变量名
    - `Type`：变量类型（VarType）
    - `Default`：默认值

---

## 枚举与类型

### VarType
- 变量类型枚举：
  - `Undefined`：未定义
  - `Moment`：瞬间
  - `Forever`：永久直到被覆盖
  - `Duration`：持续一段时间

### CampType
- 阵营类型枚举：
  - `None`：无阵营
  - `Watch`：观众
  - `Neutral`：中立
  - `Camp1`~`Camp8`：阵营1~8

### RoomType
- 房间类型枚举：
  - `None`：未定义
  - `Normal`：常规
  - `Temp`：临时

### UnitType
- 单位类型枚举：
  - `None`：未定义
  - `Building`：建筑
  - `Troop`：部队
  - `Banner`：旗帜

---

## 数学结构体

### V2Int
- 二维整型向量结构体，支持索引、构造、Set、ToString、Equals等常用操作。
  - 字段：`x`、`y`
  - 索引器：`this[int index]`
  - 构造函数：`V2Int(int x, int y)`
  - 方法：`Set(int newX, int newY)`、`GetHashCode()`、`Equals`、`ToString()`

### V3Int
- 三维整型向量结构体，支持索引、构造、Set、ToString、Equals等常用操作。
  - 字段：`x`、`y`、`z`
  - 索引器：`this[int index]`
  - 构造函数：`V3Int(int x, int y, int z)`、`V3Int(int x, int y)`
  - 方法：`Set(int newX, int newY, int newZ)`、`GetHashCode()`、`Equals`、`ToString()`
