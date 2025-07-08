namespace JLGames.RabbitClient.Server.MMO
{
    public interface ITowardSupport
    {
        /// <summary>
        /// 朝向
        /// </summary>
        int Toward { get; }

        /// <summary>
        /// 设置目标朝向
        /// </summary>
        /// <param name="towardAngleInt"></param>
        void SetTowardAngleInt(int towardAngleInt);

        /// <summary>
        /// 设置目标朝向
        /// </summary>
        /// <param name="towardAngleInt"></param>
        void SetTowardAngleInt(short towardAngleInt);
    }
}