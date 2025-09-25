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
        /// Event Data： NotifyPlayerVarData
        /// 事件数据： NotifyPlayerVarData
        /// </summary>
        public const string NotifyPlayerVar = "PlayerEvents.NotifyPlayerVar";

        /// <summary>
        /// Player vars notify del
        /// 玩家变量删除
        /// Event Data： NotifyPlayerVarDelData
        /// 事件数据： NotifyPlayerVarDelData
        /// </summary>
        public const string NotifyPlayerVarDel = "PlayerEvents.NotifyPlayerVarDel";

        /// <summary>
        /// Player pos notify
        /// 玩家坐标变更
        /// Event Data： NotifyPlayerVarData
        /// 事件数据： NotifyPlayerVarData
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
        public class NotifyPlayerVarData
        {
            public string PlayerId;
            public IVarSet VarSet;
        }

        /// <summary>
        /// Player var del event data
        /// 玩家变量删除事件数据
        /// </summary>
        public class NotifyPlayerVarDelData
        {
            public string PlayerId;
            public string[] Keys;
        }
    }
}