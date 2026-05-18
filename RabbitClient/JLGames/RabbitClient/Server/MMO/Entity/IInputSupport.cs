namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Player or unit movement and target input support.
    /// 玩家或单位的移动与目标输入支持。
    /// </summary>
    public interface IInputSupport
    {
        /// <summary>
        /// Whether move input is active.
        /// 有输入状态
        /// </summary>
        bool InputMoveOn { get; }

        /// <summary>
        /// Whether target input is active.
        /// 有目标
        /// </summary>
        bool InputTargetOn { get; }

        /// <summary>
        /// Current move input vector.
        /// 控制
        /// </summary>
        V3Int InputMoveInt { get; }

        /// <summary>
        /// Current target position vector.
        /// 目标位置
        /// </summary>
        V3Int InputTargetInt { get; }

        /// <summary>
        /// Sets move input from a vector.
        /// 设置输入状态
        /// </summary>
        /// <param name="input">Move input vector<br/>移动输入向量</param>
        void SetInputMoveInt(V3Int input);

        /// <summary>
        /// Sets move input from components.
        /// 设置输入状态
        /// </summary>
        /// <param name="x">X component<br/>X 分量</param>
        /// <param name="y">Y component<br/>Y 分量</param>
        /// <param name="z">Z component<br/>Z 分量</param>
        void SetInputMoveInt(int x, int y, int z);

        /// <summary>
        /// Sets target position from a vector.
        /// 设置目标位置
        /// </summary>
        /// <param name="input">Target position vector<br/>目标位置向量</param>
        void SetInputTargetInt(V3Int input);

        /// <summary>
        /// Sets target position from components.
        /// 设置目标位置
        /// </summary>
        /// <param name="x">X component<br/>X 分量</param>
        /// <param name="y">Y component<br/>Y 分量</param>
        /// <param name="z">Z component<br/>Z 分量</param>
        void SetInputTargetInt(int x, int y, int z);
    }
}
