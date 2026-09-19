# Namespace: JLGames.RabbitClient.Server.MMO

MMO entities, variables, manager, events, meta, and integer vectors. All public types live in this namespace (source is grouped under Entity / Event / Lang / Meta / Var).

## Entity Interfaces

### IEntity

Extends `IEquatable<IEntity>`.

- `EntityType`: Entity type
- `EntityId`: Entity id

### IEntityPlayer

Extends `IEntity`, `IEquatable<IEntityPlayer>`, `IEventDispatcher`, `IVarSupport`, `IPosSupport`, `ITowardSupport`, `IInputSupport`, `IUpdateSupport`.

- `PlayerId`, `IsSelf`, `NickName`, `TeamId`
- `SetSelfPlayerId(string selfId)`

### IEntityRoom

Extends `IEntity`, `IEquatable<IEntityRoom>`, `IEventDispatcher`, `IUpdateSupport`, `IVarSupport`.

- `RoomId`, `RoomName`, `PlayerCount`, `UnitCount`
- `FindIPlayer(string playerId)` / `FindIUnit(string unitId)`
- `ForEachPlayer(Action<int, IEntityPlayer> each)` / `ForEachUnit(Action<int, IEntityUnit> each)`

### IEntityUnit

Extends `IEntity`, `IEquatable<IEntityUnit>`, `IEventDispatcher`, `IVarSupport`, `IPosSupport`, `ITowardSupport`, `IInputSupport`, `IUpdateSupport`.

- `UnitId`, `Owner`, `RoomId`

### Capability Interfaces

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

- `InputMoveOn`, `InputTargetOn`, `InputMoveInt`, `InputTargetInt`
- `SetInputMoveInt(V3Int)` / `SetInputMoveInt(int x, int y, int z)`
- `SetInputTargetInt(V3Int)` / `SetInputTargetInt(int x, int y, int z)`

**IUpdateSupport**

- `UpdateFromReader(IRabbitResponseMsg reader)`

## Entity Implementations

### EntityPlayer

Implements `IEntityPlayer`, extends `EventDispatcher`.

- Constructor: `EntityPlayer(string playerId)`
- Static: `GenPlayerFromReader(IRabbitResponseMsg reader)`, `NewPlayer()`, `NewIPlayer()`

### EntityRoom

Implements `IEntityRoom`, extends `EventDispatcher`.

- Constructor: `EntityRoom(string roomId)`
- `SetRoomId(string roomId)`
- `FindPlayer` / `RemovePlayer` / `AddPlayer` / `AddPlayers(int addCount)`
- `FindUnit` / `RemoveUnit` / `AddUnit` / `AddUnits(int addCount)`
- Static: `GenRoomFromReader(IRabbitResponseMsg reader)`, `NewRoom()`, `NewIRoom()`

### EntityUnit

Implements `IEntityUnit`, extends `EventDispatcher`.

- Constructor: `EntityUnit(string unitId)`
- Static: `GenUnitFromReader(IRabbitResponseMsg reader)`, `NewUnit()`, `NewIUnit()`

## Variables

### IVarSet

Extends `INetMessage`.

- `Size`: All entries including timestamp keys
- `KeySize`: Business key count
- `KeyToStampKey(string key)`
- `Clear()`
- `SetVar(string key, object value)` / `SetVar(string key, object value, long timestamp)`
- `DeleteVar(string key, bool includeStampKey)`
- `SetVars(Dictionary<string, object> vars)` plus timestamp / `IVarSet` overloads
- `DeleteVars(string[] keys, bool includeStampKey)`
- `CheckKey(string key)`, `GetValue(string key)`, `GetValue<T>(string key)`, `GetValueStamp(string key)`
- `ForEach(VarSetDelegates.FuncEach)` / `ForEach(VarSetDelegates.FuncStampEach)`

Supported value types: primitives and their arrays, plus `V2Int` / `V3Int`.

### VarSet

Implements `IVarSet`.

- Constructor: `VarSet(bool littleEndian)`
- Also: `EncodeToBytes()` / `DecodeFromBytes(byte[])`, `EncodeToBuff` / `DecodeFromBuff`

### VarData&lt;T&gt;

Typed variable entry with timestamp: `Key`, `Type` (`VarType`), `Value` (assigning updates `Stamp`), `Stamp`.

### VarSetDelegates

- `FuncEach(string key, object value)`
- `FuncStampEach(string key, object value, long stamp)`

## MmoManager

Extends `EventDispatcher`. Binds an external dispatcher for room state.

- `StartManager(IEventDispatcher dispatcher)`
- `StopManager()`

## Events

### PlayerEvents

| Event | Payload |
| --- | --- |
| `EventLeaveRoom` | `NotifyPlayerLeaveRoomData` (`RoomId`, `PlayerId`, `Player`) |
| `EventEnterRoom` | `NotifyPlayerEnterRoomData` (`RoomId`, `Player`) |
| `NotifyPlayerVars` | `NotifyPlayerVarsData` (`PlayerId`, `VarSet`) |
| `NotifyPlayerDelVars` | Player variable deletion |
| `NotifyPlayerVarPos` | `NotifyPlayerVarsData` |

Also: `NotifyPlayerDelData` (`RoomId`, `Player`).

### RoomEvents

| Event | Payload |
| --- | --- |
| `EventRoomEnter` | `roomId` (`string`) |
| `EventRoomExit` | `RoomReadyData` (`OldRoomId`, `NewRoomId`, `NewRoom`) |
| `NotifyRoomVar` | `NotifyRoomVarData` (`RoomId`, `VarSet`) |
| `NotifyRoomVarDel` | `NotifyRoomVarDelData` (`RoomId`, `Keys`) |

### UnitEvents

| Event | Payload |
| --- | --- |
| `EventUnitNew` | `EventUnitNewData` (`RsCode`, `Units`) |
| `NotifyUnitVars` | `NotifyUnitVarsData` (`UnitId`, `VarSet`) |
| `NotifyUnitDelVars` | Unit variable deletion |
| `NotifyUnitVarPos` | `NotifyUnitVarsData` |
| `NotifyUnitNew` | `NotifyUnitNewData` (`RoomId`, `PlayerId`, `Unit`) |
| `NotifyUnitDel` | `NotifyUnitDelData` (`RoomId`, `Unit`) |

### WorldEvents

- `EventWorldInit`: payload is `roomId` (`string`)

## Meta and Constants

### ProtoMMOCode

MMO protocol result codes (constant names and semantics follow the source XML comments):

- `MMORoomExist` (-101, room does not exist), `MMORoomNotExist` (-102, room already exists), `MMORoomCapLimit` (-103)
- `MMOTeamCorpsExist` (-104), `MMOTeamCorpsNotExist` (-105), `MMOTeamCorpsCapLimit` (-106)
- `MMOTeamExist` (-107), `MMOTeamNotExist` (-108), `MMOTeamCapLimit` (-109)
- `MMOChanExist` (-110), `MMOChanNotExist` (-111), `MMOChanCapLimit` (-112)
- `MMOPlayerExist` (-113), `MMOPlayerNotExist` (-114), `MMOPlayerInRoom` (-115)
- `MMOUnitExist` (-116), `MMOUnitNotExist` (-117)
- `MMOIndexType` (-200), `MMOOther` (-201)

### PlayerVarKeys

`Pos`, `Toward`, `InputMove`, `InputTarget`, `InputJump`, `ActionState`, `Hp`, `Buff`, `Nick`, `Team`, `TeamCorps`

### RoomVarKeys

- `Name`: room name (string)

### UnitVarKeys

`Owner`, `Room`, `Pos`, `Toward`, `InputMove`, `InputTarget`, `InputJump`, `ActionState`

### MmoMetas

- `GetRoomVarMeta` / `GetPlayerVarMeta` / `GetUnitVarMeta`
- `RegisterRoomVarMeta` / `RegisterPlayerVarMeta` / `RegisterUnitVarMeta`
- `MetaData`: `Key`, `Type`, `Default`

## Enums

### EntityType (Flags)

`EntityUnit`, `EntityPlayer`, `EntityRoom`, `EntityTeam`, `EntityTeamCorps`, `EntityChannel`

**EntityTypeUtil**

- `EntityNone`, `EntityAll`
- `Match(this EntityType self, EntityType check)`: any overlapping bit
- `Include(this EntityType self, EntityType check)`: all bits of `check` are set

### VarType

`Undefined`, `Moment`, `Forever`, `Duration`

### CampType

`None`, `Watch`, `Neutral`, `Camp1`–`Camp8`

### RoomType

`None`, `Normal`, `Temp`

### UnitType

`None`, `Building`, `Troop`, `Banner`

## Math Structures

### V2Int

Two-dimensional integer vector. Implements `IEquatable<V2Int>`, `IFormattable`.

- Components: `x`, `y`; indexer `this[int index]`
- Constructor: `V2Int(int x, int y)`; `Set(int newX, int newY)`
- Static: `MagnitudeLevel` / `MagnitudeValue`; `zero`, `one`, `up`, `down`, `left`, `right`
- `Min` / `Max` / `Lerp` / `LerpUnclamped`
- Operators: `+`, `-`, unary `-`, `*`, `/`, `==`, `!=`

### V3Int

Three-dimensional integer vector. Implements `IEquatable<V3Int>`, `IFormattable`.

- Components: `x`, `y`, `z`; indexer `this[int index]`
- Constructors: `V3Int(int x, int y, int z)`, `V3Int(int x, int y)`; `Set(int newX, int newY, int newZ)`
- Static: `MagnitudeLevel` / `MagnitudeValue`; `zero`, `one`, `forward`, `back`, `up`, `down`, `left`, `right`
- `Min` / `Max` / `Lerp` / `LerpUnclamped`
- Operators: `+`, `-`, unary `-`, `*`, `/`, `==`, `!=`
