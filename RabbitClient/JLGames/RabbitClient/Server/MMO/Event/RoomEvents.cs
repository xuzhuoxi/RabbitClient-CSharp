namespace JLGames.RabbitClient.Server.MMO
{
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
        /// Event Data： RoomReadyData
        /// 事件数据： RoomReadyData
        /// </summary>
        public const string EventRoomExit = "RoomEvents.Exit";

        /// <summary>
        /// Room vars notify
        /// 房间变量变更
        /// Event Data： NotifyRoomVarData
        /// 事件数据： NotifyRoomVarData
        /// </summary>
        public const string NotifyRoomVar = "RoomEvents.NotifyVar";

        /// <summary>
        /// Room vars notify del
        /// 房间变量删除
        /// Event Data： NotifyRoomVarDelData
        /// 事件数据： NotifyRoomVarDelData
        /// </summary>
        public const string NotifyRoomVarDel = "RoomEvents.NotifyVarDel";

        /// <summary>
        /// Room var update event data
        /// 房间变量更新事件数据
        /// </summary>
        public class NotifyRoomVarData
        {
            public string RoomId;
            public IVarSet VarSet;
        }

        /// <summary>
        /// Room var del event data
        /// 房间变量删除事件数据
        /// </summary>
        public class NotifyRoomVarDelData
        {
            public string RoomId;
            public string[] Keys;
        }

        /// <summary>
        /// room transfer data
        /// 房间转移准备数据
        /// </summary>
        public class RoomReadyData
        {
            public string OldRoomId;
            public string NewRoomId;
            public IEntityRoom NewRoom;
        }
    }
}