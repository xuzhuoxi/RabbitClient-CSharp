using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Central manager for MMO room state and event dispatch on the client.
    /// 客户端 MMO 房间状态与事件分发的中央管理器。
    /// </summary>
    public sealed class MmoManager : EventDispatcher
    {
        private EntityRoom m_Room;
        private IVarSet m_TempVarSet;
        private IEventDispatcher m_Dispatcher;

        /// <summary>
        /// Starts the manager and binds an external event dispatcher.
        /// 启动管理器并绑定外部事件分发器。
        /// </summary>
        /// <param name="dispatcher">External event dispatcher<br/>外部事件分发器</param>
        public void StartManager(IEventDispatcher dispatcher)
        {
            m_Dispatcher = dispatcher;
            InitParams();
        }

        /// <summary>
        /// Stops the manager and releases the bound dispatcher reference.
        /// 停止管理器并释放绑定的分发器引用。
        /// </summary>
        public void StopManager()
        {
            m_Dispatcher = null;
        }

        private void InitParams()
        {
            m_Room = new EntityRoom("");
            m_TempVarSet = new VarSet(RabbitServerDefaults.LittleEndian);
        }
    }
}
