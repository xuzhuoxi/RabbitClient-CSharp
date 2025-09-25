using JLGames.Infra.Event;

namespace JLGames.RabbitClient.Server.MMO
{
    public sealed class MmoManager : EventDispatcher
    {
        private EntityRoom m_Room;
        private IVarSet m_TempVarSet;
        private IEventDispatcher m_Dispatcher;

        public void StartManager(IEventDispatcher dispatcher)
        {
            m_Dispatcher = dispatcher;
            InitParams();
        }

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