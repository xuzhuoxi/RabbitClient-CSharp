namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Constants and payload types for room-related MMO events.
    /// 与房间相关的 MMO 事件常量及事件载荷类型。
    /// </summary>
    public static class RoomEvents
    {
        /// <summary>
        /// room init
        /// 房间初始化
        /// Event Data： roomId
        /// 事件数据： roomId
        /// </summary>
        public const string EventRoomEnter = "RoomEvents.Init";

        /// <summary>
        /// room exit
        /// 房间退出
        /// Event Data： RoomReadyData <see cref="RoomReadyData"/>
        /// 事件数据： RoomReadyData <see cref="RoomReadyData"/>
        /// </summary>
        public const string EventRoomExit = "RoomEvents.Exit";

        /// <summary>
        /// Room vars notify
        /// 房间变量变更
        /// Event Data： NotifyRoomVarData <see cref="NotifyRoomVarData"/>
        /// 事件数据： NotifyRoomVarData <see cref="NotifyRoomVarData"/>
        /// </summary>
        public const string NotifyRoomVar = "RoomEvents.NotifyVar";

        /// <summary>
        /// Room vars notify del
        /// 房间变量删除
        /// Event Data： NotifyRoomVarDelData <see cref="NotifyRoomVarDelData"/>
        /// 事件数据： NotifyRoomVarDelData <see cref="NotifyRoomVarDelData"/>
        /// </summary>
        public const string NotifyRoomVarDel = "RoomEvents.NotifyVarDel";

        /// <summary>
        /// Room var update event data
        /// 房间变量更新事件数据
        /// </summary>
        public class NotifyRoomVarData
        {
            /// <summary>
            /// Identifier of the room whose variables changed.
            /// 变量发生变化的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// Snapshot or delta of updated room variables for this notify.
            /// 本次通知中更新后的房间变量集合（快照或增量）。
            /// </summary>
            public IVarSet VarSet;
        }

        /// <summary>
        /// Room var del event data
        /// 房间变量删除事件数据
        /// </summary>
        public class NotifyRoomVarDelData
        {
            /// <summary>
            /// Identifier of the room whose variables were cleared.
            /// 变量被删除的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// Keys of room variables that were removed.
            /// 被移除的房间变量键名列表。
            /// </summary>
            public string[] Keys;
        }

        /// <summary>
        /// room transfer data
        /// 房间转移准备数据
        /// </summary>
        public class RoomReadyData
        {
            /// <summary>
            /// Identifier of the room the client is leaving.
            /// 客户端正在离开的房间标识。
            /// </summary>
            public string OldRoomId;

            /// <summary>
            /// Identifier of the room the client is entering.
            /// 客户端正在进入的房间标识。
            /// </summary>
            public string NewRoomId;

            /// <summary>
            /// Room entity for the destination room, if already available.
            /// 目标房间的实体（若已可用）。
            /// </summary>
            public IEntityRoom NewRoom;
        }
    }
}
