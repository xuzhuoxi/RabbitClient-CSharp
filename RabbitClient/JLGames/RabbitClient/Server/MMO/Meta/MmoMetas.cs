using System.Collections.Generic;

namespace JLGames.RabbitClient.Server.MMO
{
    public static class MmoMetas
    {
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

        public static MetaData GetRoomVarMeta(string key)
        {
            return !s_RoomMetas.ContainsKey(key) ? null : s_RoomMetas[key];
        }

        public static MetaData GetPlayerVarMeta(string key)
        {
            return !s_PlayerMetas.ContainsKey(key) ? null : s_PlayerMetas[key];
        }

        public static MetaData GetUnitVarMeta(string key)
        {
            return !s_UnitMetas.ContainsKey(key) ? null : s_UnitMetas[key];
        }

        public static void RegisterRoomVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_RoomMetas[metaData.Key] = metaData;
        }

        public static void RegisterPlayerVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_PlayerMetas[metaData.Key] = metaData;
        }

        public static void RegisterUnitVarMeta(MetaData metaData)
        {
            if (null == metaData) return;
            s_UnitMetas[metaData.Key] = metaData;
        }
    }
}
