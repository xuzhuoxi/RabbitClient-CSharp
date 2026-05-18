using System.Collections.Generic;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Registry of default MMO variable metadata by entity kind.
    /// 按实体类型注册的 MMO 变量默认元数据。
    /// </summary>
    public static class MmoMetas
    {
        /// <summary>
        /// Metadata for a single variable key.
        /// 单个变量键的元数据。
        /// </summary>
        public class MetaData
        {
            /// <summary>
            /// Var key
            /// 变量名
            /// </summary>
            public string Key { get; internal set; }

            /// <summary>
            /// Var type
            /// 变量类型
            /// </summary>
            public VarType Type { get; internal set; }

            /// <summary>
            /// Default value.
            /// 默认值
            /// </summary>
            public object Default { get; internal set; }
        }


        private static readonly Dictionary<string, MetaData> s_RoomMetas = new Dictionary<string, MetaData>
        {
            { RoomVarKeys.Name, new MetaData { Key = RoomVarKeys.Name, Type = VarType.Forever, Default = "" } },
        };

        private static readonly Dictionary<string, MetaData> s_PlayerMetas = new Dictionary<string, MetaData>
        {
            { PlayerVarKeys.Pos, new MetaData { Key = PlayerVarKeys.Pos, Type = VarType.Forever, Default = V3Int.zero } },
            { PlayerVarKeys.InputMove, new MetaData { Key = PlayerVarKeys.InputMove, Type = VarType.Forever, Default = V3Int.zero } },
            { PlayerVarKeys.InputJump, new MetaData { Key = PlayerVarKeys.InputJump, Type = VarType.Moment, Default = default(bool) } },

            { PlayerVarKeys.Toward, new MetaData { Key = PlayerVarKeys.Toward, Type = VarType.Forever, Default = default(byte) } },
            { PlayerVarKeys.ActionState, new MetaData { Key = PlayerVarKeys.ActionState, Type = VarType.Moment, Default = default(uint) } },

            { PlayerVarKeys.Hp, new MetaData { Key = PlayerVarKeys.Hp, Type = VarType.Forever, Default = default(uint) } },
            { PlayerVarKeys.Buff, new MetaData { Key = PlayerVarKeys.Buff, Type = VarType.Forever, Default = default(uint) } },
            { PlayerVarKeys.Nick, new MetaData { Key = PlayerVarKeys.Nick, Type = VarType.Forever, Default = "" } },
            { PlayerVarKeys.Team, new MetaData { Key = PlayerVarKeys.Team, Type = VarType.Forever, Default = "" } },
            { PlayerVarKeys.TeamCorps, new MetaData { Key = PlayerVarKeys.TeamCorps, Type = VarType.Forever, Default = "" } },
        };

        private static readonly Dictionary<string, MetaData> s_UnitMetas = new Dictionary<string, MetaData>
        {
            { UnitVarKeys.Owner, new MetaData { Key = UnitVarKeys.Owner, Type = VarType.Forever, Default = default(string) } },
            { UnitVarKeys.Toward, new MetaData { Key = UnitVarKeys.Toward, Type = VarType.Forever, Default = V3Int.zero } },
            { UnitVarKeys.Pos, new MetaData { Key = UnitVarKeys.Pos, Type = VarType.Forever, Default = V3Int.zero } },

            { UnitVarKeys.InputMove, new MetaData { Key = UnitVarKeys.InputMove, Type = VarType.Forever, Default = V3Int.zero } },
            { UnitVarKeys.InputJump, new MetaData { Key = UnitVarKeys.InputJump, Type = VarType.Moment, Default = default(bool) } },
        };

        /// <summary>
        /// Gets room variable metadata by key.
        /// 按键获取房间变量元数据。
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Metadata or null<br/>元数据或 null</returns>
        public static MetaData GetRoomVarMeta(string key)
        {
            return !s_RoomMetas.ContainsKey(key) ? null : s_RoomMetas[key];
        }

        /// <summary>
        /// Gets player variable metadata by key.
        /// 按键获取玩家变量元数据。
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Metadata or null<br/>元数据或 null</returns>
        public static MetaData GetPlayerVarMeta(string key)
        {
            return !s_PlayerMetas.ContainsKey(key) ? null : s_PlayerMetas[key];
        }

        /// <summary>
        /// Gets unit variable metadata by key.
        /// 按键获取单位变量元数据。
        /// </summary>
        /// <param name="key">Variable key<br/>变量键</param>
        /// <returns>Metadata or null<br/>元数据或 null</returns>
        public static MetaData GetUnitVarMeta(string key)
        {
            return !s_UnitMetas.ContainsKey(key) ? null : s_UnitMetas[key];
        }

        /// <summary>
        /// Registers or replaces room variable metadata.
        /// 注册或替换房间变量元数据。
        /// </summary>
        /// <param name="metaData">Metadata entry<br/>元数据项</param>
        public static void RegisterRoomVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_RoomMetas[metaData.Key] = metaData;
        }

        /// <summary>
        /// Registers or replaces player variable metadata.
        /// 注册或替换玩家变量元数据。
        /// </summary>
        /// <param name="metaData">Metadata entry<br/>元数据项</param>
        public static void RegisterPlayerVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_PlayerMetas[metaData.Key] = metaData;
        }

        /// <summary>
        /// Registers or replaces unit variable metadata.
        /// 注册或替换单位变量元数据。
        /// </summary>
        /// <param name="metaData">Metadata entry<br/>元数据项</param>
        public static void RegisterUnitVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_UnitMetas[metaData.Key] = metaData;
        }
    }
}
