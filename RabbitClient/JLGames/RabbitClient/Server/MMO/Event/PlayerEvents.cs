namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Constants and payload types for player-related MMO events.
    /// 与玩家相关的 MMO 事件常量及事件载荷类型。
    /// </summary>
    public static class PlayerEvents
    {
        /// <summary>
        /// Player leave room
        /// 玩家离开房间
        /// Event Data： NotifyPlayerLeaveRoomData <see cref="NotifyPlayerLeaveRoomData"/>
        /// 事件数据： NotifyPlayerLeaveRoomData <see cref="NotifyPlayerLeaveRoomData"/>
        /// </summary>
        public const string EventLeaveRoom = "PlayerEvents.EventLeaveRoom";

        /// <summary>
        /// Player enter room
        /// 玩家进入房间
        /// Event Data： NotifyPlayerEnterRoomData <see cref="NotifyPlayerEnterRoomData"/>
        /// 事件数据： NotifyPlayerEnterRoomData <see cref="NotifyPlayerEnterRoomData"/>
        /// </summary>
        public const string EventEnterRoom = "PlayerEvents.EventEnterRoom";

        /// <summary>
        /// Player vars notify
        /// 玩家变量变更
        /// Event Data： NotifyPlayerVarsData <see cref="NotifyPlayerVarsData"/>
        /// 事件数据： NotifyPlayerVarsData <see cref="NotifyPlayerVarsData"/>
        /// </summary>
        public const string NotifyPlayerVars = "PlayerEvents.NotifyPlayerVars";

        /// <summary>
        /// Player vars notify del
        /// 玩家变量删除
        /// Event Data： NotifyPlayerDelVars <see cref="NotifyPlayerDelVars"/>
        /// 事件数据： NotifyPlayerDelVars <see cref="NotifyPlayerDelVars"/>
        /// </summary>
        public const string NotifyPlayerDelVars = "PlayerEvents.NotifyPlayerDelVars";

        /// <summary>
        /// Player pos notify
        /// 玩家坐标变更
        /// Event Data： NotifyPlayerVarsData <see cref="NotifyPlayerVarsData"/>
        /// 事件数据： NotifyPlayerVarsData <see cref="NotifyPlayerVarsData"/>
        /// </summary>
        public const string NotifyPlayerVarPos = "PlayerEvents.NotifyPlayerVarPos";

        /// <summary>
        /// Player leave room event data
        /// 玩家离开房间事件数据
        /// </summary>
        public class NotifyPlayerLeaveRoomData
        {
            /// <summary>
            /// Identifier of the room the player left.
            /// 玩家离开的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// Identifier of the player who left.
            /// 离开房间的玩家标识。
            /// </summary>
            public string PlayerId;

            /// <summary>
            /// Player entity instance associated with this event.
            /// 与该事件关联的玩家实体实例。
            /// </summary>
            public IEntityPlayer Player;
        }

        /// <summary>
        /// Player enter room event data
        /// 玩家进入房间事件数据
        /// </summary>
        public class NotifyPlayerEnterRoomData
        {
            /// <summary>
            /// Identifier of the room the player entered.
            /// 玩家进入的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// Player entity instance that entered the room.
            /// 进入房间的玩家实体实例。
            /// </summary>
            public IEntityPlayer Player;
        }

        /// <summary>
        /// notify del player event data
        /// 通知删除玩家事件数据
        /// </summary>
        public class NotifyPlayerDelData
        {
            /// <summary>
            /// Room identifier from which the player was removed.
            /// 玩家被移除的房间标识。
            /// </summary>
            public string RoomId;

            /// <summary>
            /// The player entity that was deleted or removed.
            /// 被删除或移除的玩家实体。
            /// </summary>
            public IEntityPlayer Player;
        }

        /// <summary>
        /// Player var update event data
        /// 玩家变量更新事件数据
        /// </summary>
        public class NotifyPlayerVarsData
        {
            /// <summary>
            /// Identifier of the player whose variables changed.
            /// 变量发生变化的玩家标识。
            /// </summary>
            public string PlayerId;

            /// <summary>
            /// Snapshot or delta of updated player variables for this notify.
            /// 本次通知中更新后的玩家变量集合（快照或增量）。
            /// </summary>
            public IVarSet VarSet;
        }
    }
}
