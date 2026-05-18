namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Facing direction support for entities.
    /// 实体朝向支持。
    /// </summary>
    public interface ITowardSupport
    {
        /// <summary>
        /// Current facing angle as integer.
        /// 朝向
        /// </summary>
        int Toward { get; }

        /// <summary>
        /// Sets facing angle from an integer.
        /// 设置目标朝向
        /// </summary>
        /// <param name="towardAngleInt">Facing angle<br/>朝向角度</param>
        void SetTowardAngleInt(int towardAngleInt);

        /// <summary>
        /// Sets facing angle from a short.
        /// 设置目标朝向
        /// </summary>
        /// <param name="towardAngleInt">Facing angle<br/>朝向角度</param>
        void SetTowardAngleInt(short towardAngleInt);
    }
}
