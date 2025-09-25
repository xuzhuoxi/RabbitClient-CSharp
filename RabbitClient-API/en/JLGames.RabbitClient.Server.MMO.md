# Namespace: JLGames.RabbitClient.Server.MMO

This namespace contains interfaces and implementations related to MMO entities, variables, management, events, metadata, type constants, mathematical structures, etc.

## Interfaces and Classes

### IEntity
- Description: MMO entity base interface.
- Main Properties:
  - `EntityType`: Entity type.
  - `EntityId`: Entity id.

### IEntityPlayer
- Description: Player entity interface, inherits from IEntity, IEquatable<IEntityPlayer>, IEventDispatcher, IVarSupport, IPosSupport, ITowardSupport, IInputSupport, IUpdateSupport.
- Main Properties:
  - `PlayerId`: Player Id.
  - `IsSelf`: Is it the current player.
  - `NickName`: Player nick name.
  - `TeamId`: Team Id.
- Main Methods:
  - `SetSelfPlayerId(string selfId)`: Set current player Id.

### IEntityRoom
- Description: Room entity interface, inherits from IEntity, IEquatable<IEntityRoom>, IEventDispatcher, IUpdateSupport, IVarSupport.
- Main Properties:
  - `RoomId`: Room Id.
  - `RoomName`: Room name.
  - `PlayerCount`: Player count.
  - `UnitCount`: Unit count.
- Main Methods:
  - `FindIPlayer(string playerId)`: Find player information.
  - `FindIUnit(string unitId)`: Find unit information.
  - `ForEachPlayer(Action<int, IEntityPlayer> each)`: Iterate through each player.
  - `ForEachUnit(Action<int, IEntityUnit> each)`: Iterate through each unit.

### IEntityUnit
- Description: Unit entity interface, inherits from IEntity, IEquatable<IEntityUnit>, IEventDispatcher, IVarSupport, IPosSupport, ITowardSupport, IInputSupport, IUpdateSupport.
- Main Properties:
  - `UnitId`: Unit Id.
  - `Owner`: Owner.
  - `RoomId`: Room Id where it belongs.

### IVarSet
- Description: Variable collection interface, inherits from INetMessage.
- Main Properties:
  - `Size`: All var size.
  - `KeySize`: Key var size.
- Main Methods:
  - `KeyToStampKey(string key)`: Variable key to variable timestamp key.
  - `Clear()`: Clear variable collection.
  - `SetVar(string key, object value)`: Set variable value.
  - `SetVar(string key, object value, long timestamp)`: Set variable value (with timestamp).
  - `DeleteVar(string key, bool includeStampKey)`: Delete variable value and return it.
  - `SetVars(Dictionary<string, object> vars)`: Batch set variable values.
  - `SetVars(Dictionary<string, object> vars, long timestamp)`: Batch set variable values (with timestamp).
  - `SetVars(IVarSet set)`: Batch set variable values (from another IVarSet).
  - `SetVars(IVarSet set, long timestamp)`: Batch set variable values (with timestamp).
  - `DeleteVars(string[] keys, bool includeStampKey)`: Batch delete variables.
  - `CheckKey(string key)`: Check if key is included.
  - `GetValue(string key)`: Get value.
  - `GetValue<T>(string key)`: Get generic value.
  - `GetValueStamp(string key)`: Get timestamp when value was set.
  - `ForEach(VarSetDelegates.FuncEach each)`: Iterate through variables.
  - `ForEach(VarSetDelegates.FuncStampEach stampEach)`: Iterate through variables (with timestamp).

### IVarSupport
- Description: Variable support interface.
- Main Properties:
  - `VarSet`: Variable collection.
- Main Methods:
  - `SetVar(string key, object value)`: Set local property.
  - `SetVars(IVarSet vars)`: Batch set local properties.
  - `DelVar(string key)`: Delete local property.
  - `DelVars(string[] keys)`: Batch delete local properties.

### IPosSupport
- Description: Position support interface.
- Main Properties:
  - `PosInt`: Position.
- Main Methods:
  - `SetPosInt(V3Int xyz)`: Set coordinates.
  - `SetPosInt(int x, int y, int z)`: Set coordinates.

### ITowardSupport
- Description: Direction support interface.
- Main Properties:
  - `Toward`: Direction.
- Main Methods:
  - `SetTowardAngleInt(int towardAngleInt)`: Set target direction.
  - `SetTowardAngleInt(short towardAngleInt)`: Set target direction.

### IInputSupport
- Description: Input support interface.
- Main Properties:
  - `InputMoveOn`: Has input state.
  - `InputTargetOn`: Has target.
  - `InputMoveInt`: Control.
  - `InputTargetInt`: Target position.
- Main Methods:
  - `SetInputMoveInt(V3Int input)`: Set input state.
  - `SetInputMoveInt(int x, int y, int z)`: Set input state.
  - `SetInputTargetInt(V3Int input)`: Set target position.
  - `SetInputTargetInt(int x, int y, int z)`: Set target position.

### IUpdateSupport
- Description: Update support interface.
- Main Methods:
  - `UpdateFromReader(IRabbitResponseMsg reader)`: Update data from an IRabbitResponseMsg object.

### MmoManager
- Description: MMO manager, inherits from EventDispatcher.
- Main Methods:
  - `StartManager(IEventDispatcher dispatcher)`: Start manager.
  - `StopManager()`: Stop manager.

### VarSet
- Description: Variable collection implementation, supports storage, batch operations, serialization of various types of variables.
- Main Methods:
  - See IVarSet interface definition for details.
  - `EncodeToBytes()`: Encode to byte array.
  - `DecodeFromBytes(byte[] bytes)`: Decode from byte array.
  - `EncodeToBuff(IDataBufferWriter buff)`: Encode to buffer.
  - `DecodeFromBuff(IDataBufferReader buff)`: Decode from buffer.

### EntityType (Enum)
- Description: Entity type.
- Enum Values:
  - `EntityUnit`: Unit entity.
  - `EntityPlayer`: Player entity.
  - `EntityRoom`: Room entity.
  - `EntityTeam`: Team entity.
  - `EntityTeamCorps`: Team corps entity.
  - `EntityChannel`: Channel entity.
- Related Static Tools:
  - `EntityTypeUtil.EntityNone`: No type.
  - `EntityTypeUtil.EntityAll`: All types.
  - `EntityTypeUtil.Match(EntityType self, EntityType check)`: Check if self is a part of check.
  - `EntityTypeUtil.Include(EntityType self, EntityType check)`: Check if self contains check.

---

## Events and Event Data Structures

### PlayerEvents
- Event Constants:
  - `EventLeaveRoom`: Player leaves room (event data: LeaveRoomData)
  - `EventEnterRoom`: Player enters room (event data: EnterRoomData)
  - `NotifyPlayerVar`: Player variable change (event data: NotifyPlayerVarData)
  - `NotifyPlayerVarDel`: Player variable deletion (event data: NotifyPlayerVarDelData)
  - `NotifyPlayerVarPos`: Player coordinate change (event data: NotifyPlayerVarData)
- Event Data Structures:
  - `NotifyPlayerLeaveRoomData`: RoomId, PlayerId, IEntityPlayer
  - `NotifyPlayerEnterRoomData`: RoomId, IEntityPlayer
  - `NotifyPlayerVarData`: PlayerId, IVarSet
  - `NotifyPlayerVarDelData`: PlayerId, Keys

### RoomEvents
- Event Constants:
  - `EventRoomEnter`: Room initialization (data: roomId)
  - `EventRoomExit`: Room exit (data: RoomReadyData)
  - `NotifyRoomVar`: Room variable change (data: NotifyRoomVarData)
  - `NotifyRoomVarDel`: Room variable deletion (data: NotifyRoomVarDelData)
- Event Data Structures:
  - `NotifyRoomVarData`: RoomId, IVarSet
  - `NotifyRoomVarDelData`: RoomId, Keys
  - `RoomReadyData`: OldRoomId, NewRoomId, IEntityRoom

### UnitEvents
- Event Constants:
  - `EventUnitNew`: Player creates new unit (data: EventUnitNewData)
  - `NotifyUnitVar`: Unit variable change (data: NotifyUnitVarData)
  - `NotifyUnitVarDel`: Unit variable deletion (data: NotifyUnitVarDelData)
  - `NotifyUnitVarPos`: Unit coordinate change (data: NotifyUnitVarData)
  - `NotifyUnitNew`: New unit birth (data: NotifyNewUnitData)
  - `NotifyUnitDel`: Unit deleted (data: NotifyDelUnitData)
- Event Data Structures:
  - `EventUnitNewData`: RsCode, IEntityUnit[]
  - `NotifyUnitNewData`: RoomId, PlayerId, IEntityUnit
  - `NotifyUnitVarData`: UnitId, IVarSet
  - `NotifyUnitVarDelData`: UnitId, Keys
  - `NotifyUnitDelData`: RoomId, IEntityUnit

### WorldEvents
- Event Constants:
  - `EventWorldInit`: World initialization (data: roomId)

---

## Metadata and Constants

### ProtoMMOCode
- MMO related error code constants:
  - `MMORoomExist`: Room does not exist
  - `MMORoomNotExist`: Room already exists
  - `MMORoomCapLimit`: Room capacity limit
  - `MMOTeamCorpsExist`: Team does not exist
  - `MMOTeamCorpsNotExist`: Team already exists
  - `MMOTeamCorpsCapLimit`: Team capacity limit
  - `MMOTeamExist`: Squad does not exist
  - `MMOTeamNotExist`: Squad already exists
  - `MMOTeamCapLimit`: Squad capacity limit
  - `MMOChanExist`: Channel does not exist
  - `MMOChanNotExist`: Channel already exists
  - `MMOChanCapLimit`: Channel capacity limit
  - `MMOPlayerExist`: User already exists
  - `MMOPlayerNotExist`: User does not exist
  - `MMOPlayerInRoom`: User is already in room
  - `MMOUnitExist`: Unit already exists
  - `MMOUnitNotExist`: Unit does not exist
  - `MMOIndexType`: Index type mismatch
  - `MMOOther`: Other errors

### PlayerVarKeys
- Player variable Key constants:
  - `Pos`: int32 array, coordinates X, Y, Z
  - `Toward`: Direction, int16
  - `InputMove`: int32 array, input X, Y, Z
  - `InputTarget`: int32 array, target X, Y, Z
  - `InputJump`: Input state Jump (bool)
  - `ActionState`: Action state (uint32)
  - `Hp`: Durability (uint32)
  - `Buff`: Buff (uint32)
  - `Nick`: Nickname (string)
  - `Team`: Squad id (string)
  - `TeamCorps`: Corps Id (string)

### RoomVarKeys
- Room variable Key constants:
  - `Name`: Room name (string)

### UnitVarKeys
- Unit variable Key constants:
  - `Owner`: Owner
  - `Room`: All room Id
  - `Pos`: int32 array, coordinates X, Y, Z
  - `Toward`: Direction, int16
  - `InputMove`: int32 array, input X, Y, Z
  - `InputTarget`: int32 array, target X, Y, Z
  - `InputJump`: Input state Jump (bool)
  - `ActionState`: Action state (uint32)

### MmoMetas
- Static class providing metadata (MetaData: Key, Type, Default) for room, player, and unit variables, supports registration and query.
  - `GetRoomVarMeta(string key)`: Get room variable metadata
  - `GetPlayerVarMeta(string key)`: Get player variable metadata
  - `GetUnitVarMeta(string key)`: Get unit variable metadata
  - `RegisterRoomVarMeta(MetaData metaData)`: Register room variable metadata
  - `RegisterPlayerVarMeta(MetaData metaData)`: Register player variable metadata
  - `RegisterUnitVarMeta(MetaData metaData)`: Register unit variable metadata
- Internal Class:
  - `MetaData`:
    - `Key`: Variable name
    - `Type`: Variable type (VarType)
    - `Default`: Default value

---

## Enumerations and Types

### VarType
- Variable type enumeration:
  - `Undefined`: Undefined
  - `Moment`: Moment
  - `Forever`: Permanent until overwritten
  - `Duration`: Lasts for a period of time

### CampType
- Camp type enumeration:
  - `None`: No camp
  - `Watch`: Spectator
  - `Neutral`: Neutral
  - `Camp1`~`Camp8`: Camp 1~8

### RoomType
- Room type enumeration:
  - `None`: Undefined
  - `Normal`: Normal
  - `Temp`: Temporary

### UnitType
- Unit type enumeration:
  - `None`: Undefined
  - `Building`: Building
  - `Troop`: Troop
  - `Banner`: Banner

---

## Mathematical Structures

### V2Int
- Two-dimensional integer vector structure, supports indexing, construction, Set, ToString, Equals and other common operations.
  - Fields: `x`, `y`
  - Indexer: `this[int index]`
  - Constructor: `V2Int(int x, int y)`
  - Methods: `Set(int newX, int newY)`, `GetHashCode()`, `Equals`, `ToString()`

### V3Int
- Three-dimensional integer vector structure, supports indexing, construction, Set, ToString, Equals and other common operations.
  - Fields: `x`, `y`, `z`
  - Indexer: `this[int index]`
  - Constructors: `V3Int(int x, int y, int z)`, `V3Int(int x, int y)`
  - Methods: `Set(int newX, int newY, int newZ)`, `GetHashCode()`, `Equals`, `ToString()`
