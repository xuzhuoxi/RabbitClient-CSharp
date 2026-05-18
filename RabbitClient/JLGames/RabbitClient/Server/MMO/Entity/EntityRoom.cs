using System;
using JLGames.Infra.Event;
using JLGames.Infra.Pool;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Represents an MMO room with metadata, pooled players and units, and network synchronization helpers.
    /// 表示包含元数据、玩家与单位对象池及网络同步辅助的 MMO 房间。
    /// </summary>
    public class EntityRoom : EventDispatcher, IEntityRoom
    {
        private string m_RoomId;
        private readonly IVarSet m_VarSet;
        private readonly MetaObjectPool<EntityPlayer> m_Players;
        private readonly MetaObjectPool<EntityUnit> m_Units;

        // Interface Open 

        /// <summary>
        /// Gets the entity kind for this instance.
        /// 获取此实例的实体类型。
        /// </summary>
        public EntityType EntityType => EntityType.EntityRoom;

        /// <summary>
        /// Gets the stable entity identifier (same as the room identifier).
        /// 获取稳定实体标识（与房间标识相同）。
        /// </summary>
        public string EntityId => m_RoomId;

        /// <summary>
        /// Gets the room identifier.
        /// 获取房间标识。
        /// </summary>
        public string RoomId => m_RoomId;

        /// <summary>
        /// Gets the display name from the variable set, if present.
        /// 从变量集获取显示名称（若存在）。
        /// </summary>
        public string RoomName => m_VarSet.GetValue(RoomVarKeys.Name) as string;

        /// <summary>
        /// Gets the number of players currently in the pool.
        /// 获取当前池中玩家数量。
        /// </summary>
        public int PlayerCount => m_Players.Count;

        /// <summary>
        /// Gets the number of units currently in the pool.
        /// 获取当前池中单位数量。
        /// </summary>
        public int UnitCount => m_Units.Count;

        /// <summary>
        /// Gets the backing variable set for room-level attributes.
        /// 获取房间级属性的底层变量集。
        /// </summary>
        public IVarSet VarSet => m_VarSet;

        /// <summary>
        /// Finds a player by id and returns the interface view.
        /// 按标识查找玩家并返回接口视图。
        /// </summary>
        /// <param name="playerId">Player id to locate.<br/>要查找的玩家标识。</param>
        /// <returns>The matching player, or <c>null</c>.<br/>匹配的玩家，或 <c>null</c>。</returns>
        public IEntityPlayer FindIPlayer(string playerId)
        {
            return FindPlayer(playerId);
        }

        /// <summary>
        /// Finds a unit by id and returns the interface view.
        /// 按标识查找单位并返回接口视图。
        /// </summary>
        /// <param name="unitId">Unit id to locate.<br/>要查找的单位标识。</param>
        /// <returns>The matching unit, or <c>null</c>.<br/>匹配的单位，或 <c>null</c>。</returns>
        public IEntityUnit FindIUnit(string unitId)
        {
            return FindUnit(unitId);
        }

        /// <summary>
        /// Invokes a callback for each player with its pool index.
        /// 对每个玩家调用回调并传入其在池中的索引。
        /// </summary>
        /// <param name="each">Callback receiving index and player.<br/>接收索引与玩家的回调。</param>
        public void ForEachPlayer(Action<int, IEntityPlayer> each)
        {
            for (var i = 0; i < m_Players.Count; i++)
            {
                each.Invoke(i, m_Players[i]);
            }
        }

        /// <summary>
        /// Invokes a callback for each unit with its pool index.
        /// 对每个单位调用回调并传入其在池中的索引。
        /// </summary>
        /// <param name="each">Callback receiving index and unit.<br/>接收索引与单位的回调。</param>
        public void ForEachUnit(Action<int, IEntityUnit> each)
        {
            for (var i = 0; i < m_Units.Count; i++)
            {
                each.Invoke(i, m_Units[i]);
            }
        }

        /// <summary>
        /// Determines equality with another <see cref="IEntity"/> by type and id.
        /// 通过类型与标识判断与另一 <see cref="IEntity"/> 是否相等。
        /// </summary>
        /// <param name="other">The other entity, if any.<br/>另一实体（可为 null）。</param>
        /// <returns><c>true</c> if equal; otherwise <c>false</c>.<br/>相等则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public bool Equals(IEntity other)
        {
            if (null == other) return false;
            return this == other || (EntityType == other.EntityType && EntityId == other.EntityId);
        }

        /// <summary>
        /// Determines equality with another <see cref="IEntityRoom"/>.
        /// 判断与另一 <see cref="IEntityRoom"/> 是否相等。
        /// </summary>
        /// <param name="other">The other room entity, if any.<br/>另一房间实体（可为 null）。</param>
        /// <returns><c>true</c> if equal; otherwise <c>false</c>.<br/>相等则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public bool Equals(IEntityRoom other)
        {
            return Equals(other as IEntity);
        }

        // Class Open

        /// <summary>
        /// Initializes a new room with pooled players and units using factory delegates.
        /// 使用工厂委托初始化包含玩家与单位池的新房间。
        /// </summary>
        /// <param name="roomId">Initial room id.<br/>初始房间标识。</param>
        public EntityRoom(string roomId)
        {
            m_RoomId = roomId;
            m_VarSet = new VarSet(RabbitServerDefaults.LittleEndian);
            m_Players = new MetaObjectPool<EntityPlayer>(EntityPlayer.NewPlayer);
            m_Units = new MetaObjectPool<EntityUnit>(EntityUnit.NewUnit);
        }

        /// <summary>
        /// Updates the room identifier stored on this instance.
        /// 更新此实例上保存的房间标识。
        /// </summary>
        /// <param name="roomId">New room id.<br/>新的房间标识。</param>
        public void SetRoomId(string roomId)
        {
            m_RoomId = roomId;
        }

        /// <summary>
        /// Sets a single room-level variable.
        /// 设置单个房间级变量。
        /// </summary>
        /// <param name="key">Variable key.<br/>变量键。</param>
        /// <param name="value">Variable value.<br/>变量值。</param>
        public void SetVar(string key, object value)
        {
            m_VarSet.SetVar(key, value);
        }

        /// <summary>
        /// Merges variables from another set into the room variable set.
        /// 将另一变量集合并入房间变量集。
        /// </summary>
        /// <param name="vars">Source variables.<br/>源变量集。</param>
        public void SetVars(IVarSet vars)
        {
            m_VarSet.SetVars(vars);
        }

        /// <summary>
        /// Deletes a variable by key.
        /// 按键删除变量。
        /// </summary>
        /// <param name="key">Variable key to delete.<br/>要删除的变量键。</param>
        public void DelVar(string key)
        {
            m_VarSet.DeleteVar(key, true);
        }

        /// <summary>
        /// Deletes multiple variables by keys.
        /// 按键批量删除变量。
        /// </summary>
        /// <param name="keys">Keys to delete.<br/>要删除的键数组。</param>
        public void DelVars(string[] keys)
        {
            m_VarSet.DeleteVars(keys, true);
        }

        /// <summary>
        /// Finds the first player matching the given id.
        /// 查找首个匹配给定标识的玩家。
        /// </summary>
        /// <param name="playerId">Player id to locate.<br/>要查找的玩家标识。</param>
        /// <returns>The matching <see cref="EntityPlayer"/>, or <c>null</c>.<br/>匹配的 <see cref="EntityPlayer"/>，或 <c>null</c>。</returns>
        public EntityPlayer FindPlayer(string playerId)
        {
            return m_Players.FindFirst(each => each.PlayerId == playerId);
        }

        /// <summary>
        /// Removes the first player matching the given id from the pool.
        /// 从池中移除首个匹配给定标识的玩家。
        /// </summary>
        /// <param name="playerId">Player id to remove.<br/>要移除的玩家标识。</param>
        /// <returns>The removed player instance, or <c>null</c>.<br/>被移除的玩家实例，或 <c>null</c>。</returns>
        public EntityPlayer RemovePlayer(string playerId)
        {
            return m_Players.RemoveFirst((each => each.PlayerId == playerId));
        }

        /// <summary>
        /// Allocates one new player slot in the pool and returns the instance.
        /// 在池中分配一个新玩家槽位并返回实例。
        /// </summary>
        /// <returns>The newly added <see cref="EntityPlayer"/>.<br/>新加入的 <see cref="EntityPlayer"/>。</returns>
        public EntityPlayer AddPlayer()
        {
            var arr = m_Players.Add(1);
            return arr[0];
        }

        /// <summary>
        /// Allocates multiple new player slots; returns <c>null</c> when count is not positive.
        /// 分配多个新玩家槽位；当数量不大于 0 时返回 <c>null</c>。
        /// </summary>
        /// <param name="addCount">Number of players to add.<br/>要添加的玩家数量。</param>
        /// <returns>Array of new players, or <c>null</c>.<br/>新玩家数组，或 <c>null</c>。</returns>
        public EntityPlayer[] AddPlayers(int addCount)
        {
            return addCount <= 0 ? null : m_Players.Add(addCount);
        }

        /// <summary>
        /// Finds the first unit matching the given id.
        /// 查找首个匹配给定标识的单位。
        /// </summary>
        /// <param name="unitId">Unit id to locate.<br/>要查找的单位标识。</param>
        /// <returns>The matching <see cref="EntityUnit"/>, or <c>null</c>.<br/>匹配的 <see cref="EntityUnit"/>，或 <c>null</c>。</returns>
        public EntityUnit FindUnit(string unitId)
        {
            return m_Units.FindFirst(each => each.UnitId == unitId);
        }

        /// <summary>
        /// Removes the first unit matching the given id from the pool.
        /// 从池中移除首个匹配给定标识的单位。
        /// </summary>
        /// <param name="unitId">Unit id to remove.<br/>要移除的单位标识。</param>
        /// <returns>The removed unit instance, or <c>null</c>.<br/>被移除的单位实例，或 <c>null</c>。</returns>
        public EntityUnit RemoveUnit(string unitId)
        {
            return m_Units.RemoveFirst((each => each.UnitId == unitId));
        }

        /// <summary>
        /// Allocates one new unit slot in the pool and returns the instance.
        /// 在池中分配一个新单位槽位并返回实例。
        /// </summary>
        /// <returns>The newly added <see cref="EntityUnit"/>.<br/>新加入的 <see cref="EntityUnit"/>。</returns>
        public EntityUnit AddUnit()
        {
            var arr = m_Units.Add(1);
            return arr[0];
        }

        /// <summary>
        /// Allocates multiple new unit slots; returns <c>null</c> when count is not positive.
        /// 分配多个新单位槽位；当数量不大于 0 时返回 <c>null</c>。
        /// </summary>
        /// <param name="addCount">Number of units to add.<br/>要添加的单位数量。</param>
        /// <returns>Array of new units, or <c>null</c>.<br/>新单位数组，或 <c>null</c>。</returns>
        public EntityUnit[] AddUnits(int addCount)
        {
            return addCount <= 0 ? null : m_Units.Add(addCount);
        }

        // ---------- ---------- ---------- ----------

        /// <summary>
        /// Reads room info, players, and units from a response reader.
        /// 从响应读取器读取房间信息、玩家与单位。
        /// </summary>
        /// <param name="reader">Message reader positioned at room payload.<br/>位于房间载荷的消息读取器。</param>
        public void UpdateFromReader(IRabbitResponseMsg reader)
        {
            UpdateInfo(reader);
            UpdatePlayers(reader);
            UpdateUnits(reader);
            // DebugUtil.Log("EntityRoom.UpdateFromReader:", RoomId, RoomName, "PlayerCount:", m_Players.Count);
        }

        private void UpdateInfo(IRabbitResponseMsg reader)
        {
            if (!reader.Next) return;
            m_RoomId = reader.ReadString();
            if (!reader.Next) return;
            var bsLen = reader.ReadLen();
            if (bsLen <= 0) return;
            var bs = reader.ReadBytes(bsLen);
            m_VarSet.DecodeFromBytes(bs);
        }

        private void UpdatePlayers(IRabbitResponseMsg reader)
        {
            if (!reader.Next) return;
            var len = reader.ReadLen();
            if (len <= 0)
            {
                m_Players.UpdateToSize(0);
                return;
            }

            m_Players.UpdateToSize(len);
            for (var index = 0; index < len; index++)
            {
                m_Players[index].UpdateFromReader(reader);
            }
        }

        private void UpdateUnits(IRabbitResponseMsg reader)
        {
            if (!reader.Next) return;
            var len = reader.ReadLen();
            if (len <= 0)
            {
                m_Units.UpdateToSize(0);
                return;
            }

            m_Units.UpdateToSize(len);
            for (var index = 0; index < len; index++)
            {
                m_Units[index].UpdateFromReader(reader);
            }
        }

        // ---------- ---------- ---------- ----------

        /// <summary>
        /// Creates a room, calls <see cref="UpdateFromReader"/> on the reader, then calls <see cref="UpdatePlayers"/> again on the same reader (as implemented).
        /// 创建房间，对读取器调用 <see cref="UpdateFromReader"/>，随后在同一读取器上再次调用 <see cref="UpdatePlayers"/>（与实现一致）。
        /// </summary>
        /// <param name="reader">Message reader for room payload.<br/>用于房间载荷的消息读取器。</param>
        /// <returns>Loaded <see cref="EntityRoom"/>.<br/>已加载的 <see cref="EntityRoom"/>。</returns>
        public static EntityRoom GenRoomFromReader(IRabbitResponseMsg reader)
        {
            var room = new EntityRoom("");
            room.UpdateFromReader(reader);
            room.UpdatePlayers(reader);
            return room;
        }

        /// <summary>
        /// Creates an empty room (temporary id) for pooling or manual population.
        /// 创建空房间（临时标识），用于对象池或手动填充。
        /// </summary>
        /// <returns>New <see cref="EntityRoom"/> with empty id.<br/>标识为空的新 <see cref="EntityRoom"/>。</returns>
        public static EntityRoom NewRoom()
        {
            return new EntityRoom("");
        }

        /// <summary>
        /// Creates an <see cref="IEntityRoom"/> wrapper around a new empty room.
        /// 基于新的空房间创建 <see cref="IEntityRoom"/>。
        /// </summary>
        /// <returns>New <see cref="IEntityRoom"/> instance.<br/>新的 <see cref="IEntityRoom"/> 实例。</returns>
        public static IEntityRoom NewIRoom()
        {
            return new EntityRoom("");
        }
    }
}
