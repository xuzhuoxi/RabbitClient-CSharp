namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// Integer grid position support for entities.
    /// 实体的整数网格坐标支持。
    /// </summary>
    public interface IPosSupport
    {
        /// <summary>
        ///  position
        ///  位置
        /// </summary>
        V3Int PosInt { get; }
        
        /// <summary>
        /// Sets position from a 3D integer vector.
        /// 设置坐标
        /// </summary>
        /// <param name="xyz">Position vector<br/>位置向量</param>
        void SetPosInt(V3Int xyz);

        /// <summary>
        /// Sets position from separate coordinates.
        /// 设置坐标
        /// </summary>
        /// <param name="x">X coordinate<br/>X 坐标</param>
        /// <param name="y">Y coordinate<br/>Y 坐标</param>
        /// <param name="z">Z coordinate<br/>Z 坐标</param>
        void SetPosInt(int x, int y, int z);
    }
}
