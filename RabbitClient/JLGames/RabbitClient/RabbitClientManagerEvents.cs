using System;

namespace JLGames.RabbitClient
{
    /// <summary>
    /// Event names and payload types for RabbitClientManager connection flow.
    /// RabbitClientManager 连接流程的事件名与载荷类型。
    /// </summary>
    public static class RabbitClientManagerEvents
    {
        /// <summary>
        /// Generic progress payload for home or server connection steps.
        /// 主页查询或服务端连接各阶段的通用进度载荷。
        /// </summary>
        /// <typeparam name="T">Step-specific result type<br/>阶段相关的结果类型</typeparam>
        public class ProgressEventData<T>
        {
            /// <summary>
            /// Whether this step succeeded.
            /// 该步骤是否成功。
            /// </summary>
            public bool Suc { get; internal set; }

            /// <summary>
            /// Step result data when available.
            /// 步骤结果数据（若有）。
            /// </summary>
            public T Data { get; internal set; }

            /// <summary>
            /// Exception when the step failed with an error.
            /// 步骤因异常失败时的错误信息。
            /// </summary>
            public Exception Error { get; internal set; }

            /// <summary>
            /// Returns a diagnostic string for logging.
            /// 返回用于日志的诊断字符串。
            /// </summary>
            /// <returns>Formatted progress info<br/>格式化后的进度信息</returns>
            public override string ToString()
            {
                return
                    $"ProgressEventData<{Data.GetType().Name}>{{Suc={Suc}, Data={Data}, Error={Error}}}";
            }
        }

        /// <summary>
        /// Connect progress event
        /// 连接进度事件
        /// Event data(事件数据)：<![CDATA[ProgressEventData<QueryResult>]]>
        /// </summary>
        public const string EventOnProgressHome = "RabbitClientManagerEvents.EventOnProgressHome";

        /// <summary>
        /// Connect progress event
        /// 连接进度事件
        /// Event data(事件数据)：<![CDATA[ProgressEventData<SocketEvents.SocketConnEventInfo>]]>
        /// </summary>
        public const string EventOnProgressServer = "RabbitClientManagerEvents.EventOnProgressServer";

        /// <summary>
        /// Connect success event
        /// 连接成功事件
        /// Event data(事件数据)：True | False
        /// </summary>
        public const string EventOnConnectFinish = "RabbitClientManagerEvents.EventOnConnectFinish";
    }
}
