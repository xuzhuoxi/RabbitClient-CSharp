namespace JLGames.RabbitClient.Server.MMO
{
    public static class UnitEvents
    {
        /// <summary>
        /// player create new unit
        /// 玩家创建单位
        /// Event Data： EventUnitNewData
        /// 事件数据： EventUnitNewData
        /// </summary>
        public const string EventUnitNew = "UnitEvents.EventUnitNew";

        /// <summary>
        /// Unit vars notify
        /// 单位变量变更
        /// Event Data： NotifyUnitVarsData
        /// 事件数据： NotifyUnitVarsData
        /// </summary>
        public const string NotifyUnitVars = "UnitEvents.NotifyUnitVars";

        /// <summary>
        /// Unit vars notify del
        /// 单位变量删除
        /// Event Data： NotifyUnitDelVars
        /// 事件数据： NotifyUnitDelVars
        /// </summary>
        public const string NotifyUnitDelVars = "UnitEvents.NotifyUnitDelVars";

        /// <summary>
        /// Unit pos notify
        /// 单位坐标变更
        /// Event Data： NotifyUnitVarsData
        /// 事件数据： NotifyUnitVarsData
        /// </summary>
        public const string NotifyUnitVarPos = "UnitEvents.NotifyUnitVarPos";

        /// <summary>
        /// A new Unit is born
        /// 新单位诞生
        /// Event Data： NotifyNewUnitData
        /// 事件数据： NotifyNewUnitData
        /// </summary>
        public const string NotifyUnitNew = "UnitEvents.NotifyUnitNew";

        /// <summary>
        /// A new Unit is born
        /// 新单位诞生
        /// Event Data： NotifyDelUnitData
        /// 事件数据： NotifyDelUnitData
        /// </summary>
        public const string NotifyUnitDel = "UnitEvents.NotifyUnitDel";

        /// <summary>
        /// player create new unit event data
        /// 玩家创建单位事件数据
        /// </summary>
        public class EventUnitNewData
        {
            public int RsCode;
            public IEntityUnit[] Units;
        }

        /// <summary>
        /// notify create new unit event data
        /// 通知创建单位 事件数据
        /// </summary>
        public class NotifyUnitNewData
        {
            public string RoomId;
            public string PlayerId;
            public IEntityUnit Unit;
        }

        /// <summary>
        /// notify del unit event data
        /// 通知删除单位事件数据
        /// </summary>
        public class NotifyUnitDelData
        {
            public string RoomId;
            public IEntityUnit Unit;
        }

        /// <summary>
        /// Unit var update event data
        /// 单位变量更新 事件数据
        /// </summary>
        public class NotifyUnitVarsData
        {
            public string UnitId;
            public IVarSet VarSet;
        }
    }
}
