// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace JLGames.RabbitClient.Server.MMO
// {
//     [Serializable]
//     [CreateAssetMenu(fileName = "MmoSettings", menuName = "RabbitClient/MMO Settings", order = 0)]
//     public class MmoSettings : ScriptableObject
//     {
//         public enum VarType
//         {
//             /// <summary>
//             /// 瞬间
//             /// </summary>
//             Moment,
//
//             /// <summary>
//             /// 永久
//             /// </summary>
//             Forever,
//
//             /// <summary>
//             /// 持续一段时间
//             /// </summary>
//             Duration
//         }
//
//         [Serializable]
//         public class VarSetting
//         {
//             public string name;
//             public VarType type;
//             public string lastedTime;
//         }
//
//         [SerializeField] private List<VarSetting> m_VarSettings;
//
//         public bool CheckVarSetting(string varKey)
//         {
//             return FindVarSettingIndex(varKey) != -1;
//         }
//
//         private int FindVarSettingIndex(string varKey)
//         {
//             if (string.IsNullOrEmpty(varKey)) return -1;
//             return m_VarSettings.FindIndex(setting => setting.name == varKey);
//         }
//     }
// }
