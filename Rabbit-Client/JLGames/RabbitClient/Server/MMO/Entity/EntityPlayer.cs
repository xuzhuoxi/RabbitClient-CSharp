using JLGames.Infra.Event;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    public class EntityPlayer : EventDispatcher, IEntityPlayer
    {
        private string m_PlayerId;
        private string m_SelfPlayerId;
        private readonly IVarSet m_VarSet;
        private V3Int m_PosInt = V3Int.zero;
        private V3Int m_InputMoveInt = V3Int.zero;
        private V3Int m_InputTargetInt = V3Int.zero;

        public EntityType EntityType => EntityType.EntityPlayer;
        public string EntityId => m_PlayerId;
        public string PlayerId => m_PlayerId;
        public bool IsSelf => PlayerId == m_SelfPlayerId;
        public IVarSet VarSet => m_VarSet;

        // 以下为扩展属性

        public string NickName => m_VarSet.GetValue(PlayerVarKeys.Nick) as string;
        public string TeamId => m_VarSet.GetValue(PlayerVarKeys.Team) as string;
        public V3Int PosInt => m_PosInt;

        public bool InputMoveOn => m_VarSet.CheckKey(PlayerVarKeys.InputMove) &&
                                   (m_InputMoveInt.x != 0 || m_InputMoveInt.y != 0 || m_InputMoveInt.z != 0);
        public bool InputTargetOn =>/* m_VarSet.CheckKey(PlayerVarKeys.InputTarget) &&*/
                                     (m_InputTargetInt.x != 0 || m_InputTargetInt.y != 0 || m_InputTargetInt.z != 0);
        public V3Int InputMoveInt => m_InputMoveInt;
        public V3Int InputTargetInt => m_InputTargetInt;
        public int Toward => m_VarSet.GetValue<short>(PlayerVarKeys.Toward);

        public bool Equals(IEntity other)
        {
            if (null == other) return false;
            return this == other || (other.EntityType == EntityType && other.EntityId == EntityId);
        }

        public bool Equals(IEntityPlayer other)
        {
            return Equals(other as IEntity);
        }

        public EntityPlayer(string playerId)
        {
            m_PlayerId = playerId;
            m_VarSet = new VarSet(RabbitServerDefaults.LittleEndian);
        }

        public void SetSelfPlayerId(string selfId)
        {
            m_SelfPlayerId = selfId;
        }

        public void SetVar(string key, object value)
        {
            m_VarSet.SetVar(key, value);
            if (key == PlayerVarKeys.Pos)
                CachePosInt();
            if (key == PlayerVarKeys.InputMove)
                CacheInputMoveInt();
            if (key == PlayerVarKeys.InputTarget)
                CacheInputTargetInt();
        }

        public void SetVars(IVarSet vars)
        {
            if (null == vars || vars.Size == 0) return;
            m_VarSet.SetVars(vars);
            if (vars.CheckKey(PlayerVarKeys.Pos))
                CachePosInt();
            if (vars.CheckKey(PlayerVarKeys.InputMove))
                CacheInputMoveInt();
            if (vars.CheckKey(PlayerVarKeys.InputTarget))
                CacheInputTargetInt();
        }

        public void DelVar(string key)
        {
            m_VarSet.DeleteVar(key, true);
        }

        public void DelVars(string[] keys)
        {
            m_VarSet.DeleteVars(keys, true);
        }

        public void SetPosInt(V3Int xyz)
        {
            SetPosInt(xyz.x, xyz.y, xyz.z);
        }

        public void SetPosInt(int x, int y, int z)
        {
            m_PosInt.x = x;
            m_PosInt.y = y;
            m_PosInt.z = z;
            m_VarSet.SetVar(PlayerVarKeys.Pos, m_PosInt);
        }

        public void SetInputMoveInt(V3Int input)
        {
            SetInputMoveInt(input.x, input.y, input.z);
        }

        public void SetInputMoveInt(int x, int y, int z)
        {
            m_InputMoveInt.x = x;
            m_InputMoveInt.y = y;
            m_InputMoveInt.z = z;
            m_VarSet.SetVar(PlayerVarKeys.InputMove, m_InputMoveInt);
        }

        public void SetInputTargetInt(V3Int input)
        {
            SetInputTargetInt(input.x, input.y, input.z);
        }

        public void SetInputTargetInt(int x, int y, int z)
        {
            m_InputTargetInt.x = x;
            m_InputTargetInt.y = y;
            m_InputTargetInt.z = z;
            m_VarSet.SetVar(PlayerVarKeys.InputMove, m_InputTargetInt);
        }

        public void SetTowardAngleInt(short towardAngleInt)
        {
            SetTowardAngleInt((int)towardAngleInt);
        }

        public void SetTowardAngleInt(int towardAngleInt)
        {
            m_VarSet.SetVar(PlayerVarKeys.Toward, (short)towardAngleInt);
        }

        private void CachePosInt()
        {
            m_PosInt = m_VarSet.GetValue<V3Int>(PlayerVarKeys.Pos);
        }

        private void CacheInputMoveInt()
        {
            m_InputMoveInt = m_VarSet.GetValue<V3Int>(PlayerVarKeys.InputMove);
        }

        private void CacheInputTargetInt()
        {
            m_InputTargetInt = m_VarSet.GetValue<V3Int>(PlayerVarKeys.InputTarget);
        }

        // ---------- ---------- ---------- ----------

        public void UpdateFromReader(IRabbitResponseMsg reader)
        {
            m_PlayerId = reader.ReadString();
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

        public static EntityPlayer GenPlayerFromReader(IRabbitResponseMsg reader)
        {
            var player = new EntityPlayer("");
            player.UpdateFromReader(reader);
            return player;
        }

        public static EntityPlayer NewPlayer()
        {
            return new EntityPlayer("");
        }

        public static IEntityPlayer NewIPlayer()
        {
            return new EntityPlayer("");
        }
    }
}