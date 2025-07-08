namespace JLGames.RabbitClient.Server.MMO
{
    public interface IInputSupport
    {
        /// <summary>
        /// 有输入状态
        /// </summary>
        bool InputMoveOn { get; }

        /// <summary>
        /// 有目标
        /// </summary>
        bool InputTargetOn { get; }

        /// <summary>
        /// 控制
        /// </summary>
        V3Int InputMoveInt { get; }

        /// <summary>
        /// 目标位置
        /// </summary>
        V3Int InputTargetInt { get; }

        /// <summary>
        /// 设置输入状态
        /// </summary>
        /// <param name="input"></param>
        void SetInputMoveInt(V3Int input);

        /// <summary>
        /// 设置输入状态
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetInputMoveInt(int x, int y, int z);

        /// <summary>
        /// 设置目标位置
        /// </summary>
        /// <param name="input"></param>
        void SetInputTargetInt(V3Int input);

        /// <summary>
        /// 设置目标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetInputTargetInt(int x, int y, int z);
    }
}