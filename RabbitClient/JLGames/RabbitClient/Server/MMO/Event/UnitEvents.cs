namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Constants and payload types for unit-related MMO events.
    /// 与单位相关的 MMO 事件常量及事件载荷类型。
    /// </summary>
    public static class UnitEvents
    {
        /// <summary>
        /// player create new unit
        /// 玩家创建单位
        /// Event Data： EventUnitNewData <see cref="EventUnitNewData"/>
        /// 事件数据： EventUnitNewData <see cref="EventUnitNewData"/>
        /// </summary>
        public const string EventUnitNew = "UnitEvents.EventUnitNew";

        /// <summary>
        /// Unit vars notify
        /// 单位变量变更
        /// Event Data： NotifyUnitVarsData <see cref="NotifyUnitVarsData"/>
        /// 事件数据： NotifyUnitVarsData <see cref="NotifyUnitVarsData"/>
        /// </summary>
        public const string NotifyUnitVars = "UnitEvents.NotifyUnitVars";

        /// <summary>
        /// Unit vars notify del
        /// 单位变量删除
        /// Event Data： NotifyUnitDelVars <see cref="NotifyUnitDelVars"/>
        /// 事件数据： NotifyUnitDelVars <see cref="NotifyUnitDelVars"/>
        /// </summary>
        public const string NotifyUnitDelVars = "UnitEvents.NotifyUnitDelVars";

        /// <summary>
        /// Unit pos notify
        /// 单位坐标变更
        /// Event Data： NotifyUnitVarsData <see cref="NotifyUnitVarsData"/>
        /// 事件数据： NotifyUnitVarsData <see cref="NotifyUnitVarsData"/>
        /// </summary>
        public const string NotifyUnitVarPos = "UnitEvents.NotifyUnitVarPos";

        /// <summary>
        /// A new Unit is born
        /// 新单位诞生
        /// Event Data： NotifyUnitNewData <see cref="NotifyUnitNewData"/>
        /// 事件数据： NotifyUnitNewData <see cref="NotifyUnitNewData"/>
        /// </summary>
        public const string NotifyUnitNew = "UnitEvents.NotifyUnitNew";

        /// <summary>
        /// A new Unit is born
        /// 新单位诞生
        /// Event Data： NotifyUnitDelData <see cref="NotifyUnitDelData"/>
        /// 事件数据： NotifyUnitDelData <see cref="NotifyUnitDelData"/>
        /// </summary>
        public const string NotifyUnitDel = "UnitEvents.NotifyUnitDel";

        /// <summary>
        /// player create new unit event data
        /// 玩家创建单位事件数据
        /// </summary>
        public class EventUnitNewData
        {
            /// <summary>
            /// Result or protocol status code returned for the create-units operation.
            /// 创建单位操作返回的结果或协议状态码。
            /// </summary>
            public int RsCode;

            /// <summary>
            /// Units created by the operation.
            /// 本次操作创建的单位列表。
            /// </summary>
            public IEntityUnit[] Units;
        }

        /// <summary>
        /// notify create new unit event data
        /// 通知创建单位 事件数据
        /// </summary>
        public class NotifyUnitNewData
        {
            /// <summary>
            /// Room identifier where the new unit appears.
            /// 新单位所在房间的标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// Player identifier responsible for creating the unit, if applicable.
            /// 创建该单位的玩家标识（若有）。
            /// </summary>
            public string PlayerId;

            /// <summary>
            /// The newly created unit entity.
            /// 新建的单位实体。
            /// </summary>
            public IEntityUnit Unit;
        }

        /// <summary>
        /// notify del unit event data
        /// 通知删除单位事件数据
        /// </summary>
        public class NotifyUnitDelData
        {
            /// <summary>
            /// Room identifier from which the unit was removed.
            /// 单位被移除的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// The unit entity that was deleted or removed.
            /// 被删除或移除的单位实体。
            /// </summary>
            public IEntityUnit Unit;
        }

        /// <summary>
        /// Unit var update event data
        /// 单位变量更新 事件数据
        /// </summary>
        public class NotifyUnitVarsData
        {
            /// <summary>
            /// Identifier of the unit whose variables changed.
            /// 变量发生变化的单位标识。
            /// </summary>
            public string UnitId;

            /// <summary>
            /// Snapshot or delta of updated unit variables for this notify.
            /// 本次通知中更新后的单位变量集合（快照或增量）。
            /// </summary>
            public IVarSet VarSet;
        }
    }
}
