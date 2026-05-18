using JLGames.Infra.Event;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Represents an MMO unit entity backed by a variable set, position and input caches, and network deserialization.
    /// 表示基于变量集、位置与输入缓存及网络反序列化的 MMO 单位实体。
    /// </summary>
    public class EntityUnit : EventDispatcher, IEntityUnit
    {
        private string m_UnitId;
        private readonly IVarSet m_VarSet;
        private V3Int m_PosInt = V3Int.zero;
        private V3Int m_InputMoveInt = V3Int.zero;
        private V3Int m_InputTargetInt = V3Int.zero;

        /// <summary>
        /// Gets the entity kind for this instance.
        /// 获取此实例的实体类型。
        /// </summary>
        public EntityType EntityType => EntityType.EntityUnit;

        /// <summary>
        /// Gets the stable entity identifier (same as the unit identifier).
        /// 获取稳定实体标识（与单位标识相同）。
        /// </summary>
        public string EntityId => m_UnitId;

        /// <summary>
        /// Gets the unit identifier.
        /// 获取单位标识。
        /// </summary>
        public string UnitId => m_UnitId;

        /// <summary>
        /// Gets the owner id from the variable set.
        /// 从变量集获取拥有者标识。
        /// </summary>
        public string Owner => m_VarSet.GetValue<string>(UnitVarKeys.Owner);

        /// <summary>
        /// Gets the room id from the variable set.
        /// 从变量集获取房间标识。
        /// </summary>
        public string RoomId => m_VarSet.GetValue<string>(UnitVarKeys.Room);

        /// <summary>
        /// Gets the backing variable set for unit attributes.
        /// 获取用于单位属性的底层变量集。
        /// </summary>
        public IVarSet VarSet => m_VarSet;

        // 以下为扩展属性

        /// <summary>
        /// Gets the cached integer grid position.
        /// 获取缓存的整数网格位置。
        /// </summary>
        public V3Int PosInt => m_PosInt;

        /// <summary>
        /// Gets whether move input is active (non-zero vector).
        /// 获取移动输入是否有效（向量非零）。
        /// </summary>
        public bool InputMoveOn => /*m_VarSet.CheckKey(UnitVarKeys.InputMove) &&*/
            (m_InputMoveInt.x != 0 || m_InputMoveInt.y != 0 || m_InputMoveInt.z != 0);

        /// <summary>
        /// Gets whether target input is active (key present and non-zero vector).
        /// 获取目标输入是否有效（键存在且向量非零）。
        /// </summary>
        public bool InputTargetOn => m_VarSet.CheckKey(UnitVarKeys.InputTarget) &&
                                     (m_InputTargetInt.x != 0 || m_InputTargetInt.y != 0 || m_InputTargetInt.z != 0);

        /// <summary>
        /// Gets the cached move input as an integer vector.
        /// 获取缓存的移动输入整数向量。
        /// </summary>
        public V3Int InputMoveInt => m_InputMoveInt;

        /// <summary>
        /// Gets the cached target input as an integer vector.
        /// 获取缓存的目标输入整数向量。
        /// </summary>
        public V3Int InputTargetInt => m_InputTargetInt;

        /// <summary>
        /// Gets the facing value from the variable set using the player toward key as stored by this implementation.
        /// 按此实现所用的玩家朝向键从变量集获取朝向值。
        /// </summary>
        public int Toward => m_VarSet.GetValue<short>(PlayerVarKeys.Toward);

        /// <summary>
        /// Determines equality with another <see cref="IEntity"/> by type and id.
        /// 通过类型与标识判断与另一 <see cref="IEntity"/> 是否相等。
        /// </summary>
        /// <param name="other">The other entity, if any.<br/>另一实体（可为 null）。</param>
        /// <returns><c>true</c> if equal; otherwise <c>false</c>.<br/>相等则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public bool Equals(IEntity other)
        {
            if (null == other) return false;
            return this == other || (other.EntityType == EntityType && other.EntityId == EntityId);
        }

        /// <summary>
        /// Determines equality with another <see cref="IEntityUnit"/>.
        /// 判断与另一 <see cref="IEntityUnit"/> 是否相等。
        /// </summary>
        /// <param name="other">The other unit entity, if any.<br/>另一单位实体（可为 null）。</param>
        /// <returns><c>true</c> if equal; otherwise <c>false</c>.<br/>相等则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public bool Equals(IEntityUnit other)
        {
            return Equals(other as IEntity);
        }

        /// <summary>
        /// Initializes a new unit with the given identifier and an empty variable set.
        /// 使用给定标识与空变量集初始化新单位。
        /// </summary>
        /// <param name="unitId">Initial unit id.<br/>初始单位标识。</param>
        public EntityUnit(string unitId)
        {
            m_UnitId = unitId;
            m_VarSet = new VarSet(RabbitServerDefaults.LittleEndian);
        }

        /// <summary>
        /// Sets a single variable and refreshes related caches when keys match position or input.
        /// 设置单个变量；若键匹配位置或输入则刷新相关缓存。
        /// </summary>
        /// <param name="key">Variable key.<br/>变量键。</param>
        /// <param name="value">Variable value.<br/>变量值。</param>
        public void SetVar(string key, object value)
        {
            m_VarSet.SetVar(key, value);
            if (key == UnitVarKeys.Pos)
                CachePosInt();
            if (key == UnitVarKeys.InputMove)
                CacheInputMoveInt();
            if (key == UnitVarKeys.InputTarget)
                CacheInputTargetInt();
        }

        /// <summary>
        /// Merges variables from another set and refreshes caches for keys that are present.
        /// 合并另一变量集中的变量，并对出现的键刷新缓存。
        /// </summary>
        /// <param name="vars">Source variable set; ignored if null or empty.<br/>源变量集；若为 null 或为空则忽略。</param>
        public void SetVars(IVarSet vars)
        {
            if (null == vars || vars.Size == 0) return;
            m_VarSet.SetVars(vars);
            if (vars.CheckKey(UnitVarKeys.Pos))
                CachePosInt();
            if (vars.CheckKey(UnitVarKeys.InputMove))
                CacheInputMoveInt();
            if (vars.CheckKey(UnitVarKeys.InputTarget))
                CacheInputTargetInt();
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
        /// Sets the cached position from a <see cref="V3Int"/> and writes it to the variable set.
        /// 从 <see cref="V3Int"/> 设置缓存位置并写入变量集。
        /// </summary>
        /// <param name="xyz">Integer position.<br/>整数位置。</param>
        public void SetPosInt(V3Int xyz)
        {
            SetPosInt(xyz.x, xyz.y, xyz.z);
        }

        /// <summary>
        /// Sets the cached position components and writes them to the variable set.
        /// 设置缓存位置分量并写入变量集。
        /// </summary>
        /// <param name="x">X component.<br/>X 分量。</param>
        /// <param name="y">Y component.<br/>Y 分量。</param>
        /// <param name="z">Z component.<br/>Z 分量。</param>
        public void SetPosInt(int x, int y, int z)
        {
            m_PosInt.x = x;
            m_PosInt.y = y;
            m_PosInt.z = z;
            m_VarSet.SetVar(UnitVarKeys.Pos, m_PosInt);
        }

        /// <summary>
        /// Sets move input from a vector and updates the variable set.
        /// 从向量设置移动输入并更新变量集。
        /// </summary>
        /// <param name="input">Move input vector.<br/>移动输入向量。</param>
        public void SetInputMoveInt(V3Int input)
        {
            SetInputMoveInt(input.x, input.y, input.z);
        }

        /// <summary>
        /// Sets move input components and updates the variable set.
        /// 设置移动输入分量并更新变量集。
        /// </summary>
        /// <param name="x">X component.<br/>X 分量。</param>
        /// <param name="y">Y component.<br/>Y 分量。</param>
        /// <param name="z">Z component.<br/>Z 分量。</param>
        public void SetInputMoveInt(int x, int y, int z)
        {
            m_InputMoveInt.x = x;
            m_InputMoveInt.y = y;
            m_InputMoveInt.z = z;
            m_VarSet.SetVar(UnitVarKeys.InputMove, m_InputMoveInt);
        }

        /// <summary>
        /// Sets target input from a vector and updates the variable set.
        /// 从向量设置目标输入并更新变量集。
        /// </summary>
        /// <param name="input">Target input vector.<br/>目标输入向量。</param>
        public void SetInputTargetInt(V3Int input)
        {
            SetInputTargetInt(input.x, input.y, input.z);
        }

        /// <summary>
        /// Sets target input components and updates the variable set.
        /// 设置目标输入分量并更新变量集。
        /// </summary>
        /// <param name="x">X component.<br/>X 分量。</param>
        /// <param name="y">Y component.<br/>Y 分量。</param>
        /// <param name="z">Z component.<br/>Z 分量。</param>
        public void SetInputTargetInt(int x, int y, int z)
        {
            m_InputTargetInt.x = x;
            m_InputTargetInt.y = y;
            m_InputTargetInt.z = z;
            m_VarSet.SetVar(UnitVarKeys.InputTarget, m_InputTargetInt);
        }

        /// <summary>
        /// Sets the facing angle using a <see cref="short"/> value.
        /// 使用 <see cref="short"/> 设置朝向角度。
        /// </summary>
        /// <param name="towardAngleInt">Facing angle as <see cref="short"/>.<br/>以 <see cref="short"/> 表示的朝向角度。</param>
        public void SetTowardAngleInt(short towardAngleInt)
        {
            SetTowardAngleInt((int)towardAngleInt);
        }

        /// <summary>
        /// Sets the facing angle, stored as <see cref="short"/> in the variable set.
        /// 设置朝向角度，并以 <see cref="short"/> 存入变量集。
        /// </summary>
        /// <param name="towardAngleInt">Facing angle as <see cref="int"/>, cast when stored.<br/>以 <see cref="int"/> 表示的朝向角度，存储时进行转换。</param>
        public void SetTowardAngleInt(int towardAngleInt)
        {
            m_VarSet.SetVar(UnitVarKeys.Toward, (short)towardAngleInt);
        }

        private void CachePosInt()
        {
            m_PosInt = m_VarSet.GetValue<V3Int>(UnitVarKeys.Pos);
        }

        private void CacheInputMoveInt()
        {
            m_InputMoveInt = m_VarSet.GetValue<V3Int>(UnitVarKeys.InputMove);
        }

        private void CacheInputTargetInt()
        {
            m_InputTargetInt = m_VarSet.GetValue<V3Int>(UnitVarKeys.InputTarget);
        }

        /// <summary>
        /// Reads the unit id and variable blob from a response reader and refreshes caches.
        /// 从响应读取器中读取单位标识与变量字节块并刷新缓存。
        /// </summary>
        /// <param name="reader">Message reader positioned at unit payload.<br/>位于单位载荷的消息读取器。</param>
        public void UpdateFromReader(IRabbitResponseMsg reader)
        {
            m_UnitId = reader.ReadString();
            if (!reader.Next)
                return;
            var varBsLen = reader.ReadLen();
            if (varBsLen <= 0)
                return;
            var bs = reader.ReadBytes(varBsLen);
            m_VarSet.DecodeFromBytes(bs);
            CachePosInt();
            CacheInputMoveInt();
            CacheInputTargetInt();
        }

        // static ---------- ---------- ---------- ----------

        /// <summary>
        /// Creates a unit instance and populates it from a response reader, exposed as <see cref="IEntityUnit"/>.
        /// 创建单位实例并从响应读取器填充数据，以 <see cref="IEntityUnit"/> 形式返回。
        /// </summary>
        /// <param name="reader">Message reader for unit data.<br/>用于单位数据的消息读取器。</param>
        /// <returns>Loaded <see cref="IEntityUnit"/>.<br/>已加载的 <see cref="IEntityUnit"/>。</returns>
        public static IEntityUnit GenUnitFromReader(IRabbitResponseMsg reader)
        {
            var player = new EntityUnit("");
            player.UpdateFromReader(reader);
            return player;
        }

        /// <summary>
        /// Creates an empty unit (temporary id) for pooling or manual population.
        /// 创建空单位（临时标识），用于对象池或手动填充。
        /// </summary>
        /// <returns>New <see cref="EntityUnit"/> with empty id.<br/>标识为空的新 <see cref="EntityUnit"/>。</returns>
        public static EntityUnit NewUnit()
        {
            return new EntityUnit("");
        }

        /// <summary>
        /// Creates an <see cref="IEntityUnit"/> wrapper around a new empty unit.
        /// 基于新的空单位创建 <see cref="IEntityUnit"/>。
        /// </summary>
        /// <returns>New <see cref="IEntityUnit"/> instance.<br/>新的 <see cref="IEntityUnit"/> 实例。</returns>
        public static IEntityUnit NewIUnit()
        {
            return new EntityUnit("");
        }
    }
}
