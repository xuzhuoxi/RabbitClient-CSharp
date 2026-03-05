namespace JLGames.RabbitClient.Server.MMO
{
    public static class PlayerEvents
    {
        /// <summary>
        /// Player leave room
        /// 玩家离开房间
        /// Event Data： LeaveRoomData
        /// 事件数据： LeaveRoomData
        /// </summary>
        public const string EventLeaveRoom = "PlayerEvents.EventLeaveRoom";

        /// <summary>
        /// Player enter room
        /// 玩家进入房间
        /// Event Data： EnterRoomData
        /// 事件数据： EnterRoomData
        /// </summary>
        public const string EventEnterRoom = "PlayerEvents.EventEnterRoom";

        /// <summary>
        /// Player vars notify
        /// 玩家变量变更
        /// Event Data： NotifyPlayerVarsData
        /// 事件数据： NotifyPlayerVarsData
        /// </summary>
        public const string NotifyPlayerVars = "PlayerEvents.NotifyPlayerVars";

        /// <summary>
        /// Player vars notify del
        /// 玩家变量删除
        /// Event Data： NotifyPlayerDelVars
        /// 事件数据： NotifyPlayerDelVars
        /// </summary>
        public const string NotifyPlayerDelVars = "PlayerEvents.NotifyPlayerDelVars";

        /// <summary>
        /// Player pos notify
        /// 玩家坐标变更
        /// Event Data： NotifyPlayerVarsData
        /// 事件数据： NotifyPlayerVarsData
        /// </summary>
        public const string NotifyPlayerVarPos = "PlayerEvents.NotifyPlayerVarPos";

        /// <summary>
        /// Player leave room event data
        /// 玩家离开房间事件数据
        /// </summary>
        public class NotifyPlayerLeaveRoomData
        {
            public string RoomId;
            public string PlayerId;
            public IEntityPlayer Player;
        }

        /// <summary>
        /// Player enter room event data
        /// 玩家进行房间事件数据
        /// </summary>
        public class NotifyPlayerEnterRoomData
        {
            public string RoomId;
            public IEntityPlayer Player;
        }

        /// <summary>
        /// Player var update event data
        /// 玩家变量更新事件数据
        /// </summary>
        public class NotifyPlayerVarsData
        {
            public string PlayerId;
            public IVarSet VarSet;
        }
    }
}
