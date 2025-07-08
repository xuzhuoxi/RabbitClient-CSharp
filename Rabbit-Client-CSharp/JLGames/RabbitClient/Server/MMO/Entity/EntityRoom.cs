using System;
using JLGames.Infra.Event;
using JLGames.Infra.Pool;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClient.Server.MMO
{
    public class EntityRoom : EventDispatcher, IEntityRoom
    {
        private string m_RoomId;
        private readonly IVarSet m_VarSet;
        private readonly MetaObjectPool<EntityPlayer> m_Players;
        private readonly MetaObjectPool<EntityUnit> m_Units;

        // Interface Open 

        public EntityType EntityType => EntityType.EntityRoom;
        public string EntityId => m_RoomId;
        public string RoomId => m_RoomId;
        public string RoomName => m_VarSet.GetValue(RoomVarKeys.Name) as string;
        public int PlayerCount => m_Players.Count;
        public int UnitCount => m_Units.Count;
        public IVarSet VarSet => m_VarSet;

        public IEntityPlayer FindIPlayer(string playerId)
        {
            return FindPlayer(playerId);
        }

        public IEntityUnit FindIUnit(string unitId)
        {
            return FindUnit(unitId);
        }

        public void ForEachPlayer(Action<int, IEntityPlayer> each)
        {
            for (var i = 0; i < m_Players.Count; i++)
            {
                each.Invoke(i, m_Players[i]);
            }
        }

        public void ForEachUnit(Action<int, IEntityUnit> each)
        {
            for (var i = 0; i < m_Units.Count; i++)
            {
                each.Invoke(i, m_Units[i]);
            }
        }

        public bool Equals(IEntity other)
        {
            if (null == other) return false;
            return this == other || (EntityType == other.EntityType && EntityId == other.EntityId);
        }

        public bool Equals(IEntityRoom other)
        {
            return Equals(other as IEntity);
        }

        // Class Open

        public EntityRoom(string roomId)
        {
            m_RoomId = roomId;
            m_VarSet = new VarSet(RabbitServerDefaults.LittleEndian);
            m_Players = new MetaObjectPool<EntityPlayer>(EntityPlayer.NewPlayer);
            m_Units = new MetaObjectPool<EntityUnit>(EntityUnit.NewUnit);
        }

        public void SetRoomId(string roomId)
        {
            m_RoomId = roomId;
        }

        public void SetVar(string key, object value)
        {
            m_VarSet.SetVar(key, value);
        }

        public void SetVars(IVarSet vars)
        {
            m_VarSet.SetVars(vars);
        }

        public void DelVar(string key)
        {
            m_VarSet.DeleteVar(key, true);
        }

        public void DelVars(string[] keys)
        {
            m_VarSet.DeleteVars(keys, true);
        }

        public EntityPlayer FindPlayer(string playerId)
        {
            return m_Players.FindFirst(each => each.PlayerId == playerId);
        }

        public EntityPlayer RemovePlayer(string playerId)
        {
            return m_Players.RemoveFirst((each => each.PlayerId == playerId));
        }

        public EntityPlayer AddPlayer()
        {
            var arr = m_Players.Add(1);
            return arr[0];
        }

        public EntityPlayer[] AddPlayers(int addCount)
        {
            return addCount <= 0 ? null : m_Players.Add(addCount);
        }

        public EntityUnit FindUnit(string unitId)
        {
            return m_Units.FindFirst(each => each.UnitId == unitId);
        }

        public EntityUnit RemoveUnit(string unitId)
        {
            return m_Units.RemoveFirst((each => each.UnitId == unitId));
        }

        public EntityUnit AddUnit()
        {
            var arr = m_Units.Add(1);
            return arr[0];
        }

        public EntityUnit[] AddUnits(int addCount)
        {
            return addCount <= 0 ? null : m_Units.Add(addCount);
        }

        // ---------- ---------- ---------- ----------

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

        public static EntityRoom GenRoomFromReader(IRabbitResponseMsg reader)
        {
            var room = new EntityRoom("");
            room.UpdateFromReader(reader);
            room.UpdatePlayers(reader);
            return room;
        }

        public static EntityRoom NewRoom()
        {
            return new EntityRoom("");
        }

        public static IEntityRoom NewIRoom()
        {
            return new EntityRoom("");
        }
    }
}