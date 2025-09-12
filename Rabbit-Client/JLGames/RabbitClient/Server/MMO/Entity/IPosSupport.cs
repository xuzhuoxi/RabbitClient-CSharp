namespace JLGames.RabbitClient.Server.MMO
{
    public interface IPosSupport
    {
        /// <summary>
        ///  position
        ///  位置
        /// </summary>
        V3Int PosInt { get; }
        
        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="xyz"></param>
        void SetPosInt(V3Int xyz);

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetPosInt(int x, int y, int z);
    }
}