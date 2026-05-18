using System;
using System.Runtime.CompilerServices;
using JLGames.Infra.Mathx;

namespace JLGames.RabbitClient.Server.MMO
{
    public partial struct V2Int
    {
        private static uint s_MagnitudeLevel = 4;
        private static int s_MagnitudeValue = 10000;

        /// <summary>
        /// Exponent (decimal digits) for fixed magnitude scaling; updating recomputes <see cref="MagnitudeValue"/>.
        /// 固定幅值缩放的指数（十进制位数）；更新后会重新计算 <see cref="MagnitudeValue"/>。
        /// </summary>
        public static uint MagnitudeLevel
        {
            get => s_MagnitudeLevel;
            set
            {
                if (value == s_MagnitudeLevel) return;
                s_MagnitudeLevel = value;
                s_MagnitudeValue = (int)Math.Pow(10, s_MagnitudeLevel);
            }
        }

        /// <summary>
        /// Cached power-of-ten magnitude (10^<see cref="MagnitudeLevel"/>) used by axis constants.
        /// 缓存的十的幂幅值（10^<see cref="MagnitudeLevel"/>），供轴向常量使用。
        /// </summary>
        public static int MagnitudeValue => s_MagnitudeValue;

        private static readonly V2Int s_Zero = new V2Int(0, 0);
        private static readonly V2Int s_One = new V2Int(s_MagnitudeValue, s_MagnitudeValue);
        private static readonly V2Int s_Up = new V2Int(0, s_MagnitudeValue);
        private static readonly V2Int s_Down = new V2Int(0, -s_MagnitudeValue);
        private static readonly V2Int s_Left = new V2Int(-s_MagnitudeValue, 0);
        private static readonly V2Int s_Right = new V2Int(s_MagnitudeValue, 0);

        /// <summary>
        /// Vector with both components set to zero.
        /// 两个分量均为零的向量。
        /// </summary>
        public static V2Int zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Zero;
        }

        /// <summary>
        /// Vector with both components set to <see cref="MagnitudeValue"/>.
        /// 两个分量均为 <see cref="MagnitudeValue"/> 的向量。
        /// </summary>
        public static V2Int one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_One;
        }

        /// <summary>
        /// Unit-scale vector pointing toward positive Y using <see cref="MagnitudeValue"/>.
        /// 沿 Y 轴正方向、长度为 <see cref="MagnitudeValue"/> 的向量。
        /// </summary>
        public static V2Int up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Up;
        }

        /// <summary>
        /// Unit-scale vector pointing toward negative Y using <see cref="MagnitudeValue"/>.
        /// 沿 Y 轴负方向、长度为 <see cref="MagnitudeValue"/> 的向量。
        /// </summary>
        public static V2Int down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Down;
        }

        /// <summary>
        /// Unit-scale vector pointing toward negative X using <see cref="MagnitudeValue"/>.
        /// 沿 X 轴负方向、长度为 <see cref="MagnitudeValue"/> 的向量。
        /// </summary>
        public static V2Int left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Left;
        }

        /// <summary>
        /// Unit-scale vector pointing toward positive X using <see cref="MagnitudeValue"/>.
        /// 沿 X 轴正方向、长度为 <see cref="MagnitudeValue"/> 的向量。
        /// </summary>
        public static V2Int right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Right;
        }


        /// <summary>
        /// Component-wise minimum of two vectors.
        /// 两个向量各分量逐一取最小值。
        /// </summary>
        /// <param name="lhs">First operand.<br/>左操作数。</param>
        /// <param name="rhs">Second operand.<br/>右操作数。</param>
        /// <returns>Vector of per-component minimums.<br/>各分量最小值构成的新向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Min(V2Int lhs, V2Int rhs) =>
            new V2Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));

        /// <summary>
        /// Component-wise maximum of two vectors.
        /// 两个向量各分量逐一取最大值。
        /// </summary>
        /// <param name="lhs">First operand.<br/>左操作数。</param>
        /// <param name="rhs">Second operand.<br/>右操作数。</param>
        /// <returns>Vector of per-component maximums.<br/>各分量最大值构成的新向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Max(V2Int lhs, V2Int rhs) =>
            new V2Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));

        /// <summary>
        /// Linear interpolation between <paramref name="a"/> and <paramref name="b"/> with clamped <paramref name="t"/>.
        /// 在 <paramref name="a"/> 与 <paramref name="b"/> 之间线性插值，并将 <paramref name="t"/> 限制在合法范围。
        /// </summary>
        /// <param name="a">Start vector.<br/>起点向量。</param>
        /// <param name="b">End vector.<br/>终点向量。</param>
        /// <param name="t">Interpolation factor (clamped to [0,1]).<br/>插值系数（会被限制在 [0,1]）。</param>
        /// <returns>Interpolated vector with integer truncation per component.<br/>逐分量截断为整数后的插值结果。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Lerp(V2Int a, V2Int b, float t)
        {
            t = MathUtil.Clamp01(t);
            return new V2Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t));
        }

        /// <summary>
        /// Linear interpolation without clamping <paramref name="t"/>.
        /// 对 <paramref name="t"/> 不做限制的线性插值。
        /// </summary>
        /// <param name="a">Start vector.<br/>起点向量。</param>
        /// <param name="b">End vector.<br/>终点向量。</param>
        /// <param name="t">Interpolation factor.<br/>插值系数。</param>
        /// <returns>Interpolated vector with integer truncation per component.<br/>逐分量截断为整数后的插值结果。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int LerpUnclamped(V2Int a, V2Int b, float t) =>
            new V2Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t));

        /// <summary>
        /// Adds two vectors component-wise.
        /// 两个向量逐分量相加。
        /// </summary>
        /// <param name="a">Left operand.<br/>左操作数。</param>
        /// <param name="b">Right operand.<br/>右操作数。</param>
        /// <returns>Sum vector.<br/>和向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator +(V2Int a, V2Int b) => new V2Int(a.x + b.x, a.y + b.y);

        /// <summary>
        /// Subtracts <paramref name="b"/> from <paramref name="a"/> component-wise.
        /// 将 <paramref name="b"/> 从 <paramref name="a"/> 逐分量相减。
        /// </summary>
        /// <param name="a">Minuend.<br/>被减数。</param>
        /// <param name="b">Subtrahend.<br/>减数。</param>
        /// <returns>Difference vector.<br/>差向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator -(V2Int a, V2Int b) => new V2Int(a.x - b.x, a.y - b.y);

        /// <summary>
        /// Negates both components of <paramref name="a"/>.
        /// 将 <paramref name="a"/> 的两个分量取负。
        /// </summary>
        /// <param name="a">Operand.<br/>操作数。</param>
        /// <returns>Negated vector.<br/>取负后的向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator -(V2Int a) => new V2Int(-a.x, -a.y);

        /// <summary>
        /// Multiplies each component of <paramref name="a"/> by scalar <paramref name="b"/>.
        /// 将 <paramref name="a"/> 的每个分量乘以标量 <paramref name="b"/>。
        /// </summary>
        /// <param name="a">Vector operand.<br/>向量操作数。</param>
        /// <param name="b">Scalar multiplier.<br/>标量乘数。</param>
        /// <returns>Scaled vector.<br/>缩放后的向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator *(V2Int a, int b) => new V2Int(a.x * b, a.y * b);

        /// <summary>
        /// Multiplies each component of <paramref name="a"/> by scalar <paramref name="b"/> (scalar-left form).
        /// 标量在左的形式：将 <paramref name="a"/> 的每个分量乘以标量 <paramref name="b"/>。
        /// </summary>
        /// <param name="b">Scalar multiplier.<br/>标量乘数。</param>
        /// <param name="a">Vector operand.<br/>向量操作数。</param>
        /// <returns>Scaled vector.<br/>缩放后的向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator *(int b, V2Int a) => new V2Int(a.x * b, a.y * b);

        /// <summary>
        /// Divides each component of <paramref name="a"/> by scalar <paramref name="b"/>.
        /// 将 <paramref name="a"/> 的每个分量除以标量 <paramref name="b"/>。
        /// </summary>
        /// <param name="a">Vector dividend.<br/>被除向量。</param>
        /// <param name="b">Scalar divisor.<br/>标量除数。</param>
        /// <returns>Quotient vector.<br/>商向量。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator /(V2Int a, int b) => new V2Int(a.x / b, a.y / b);

        /// <summary>
        /// Equality comparison for two vectors.
        /// 比较两个向量是否相等。
        /// </summary>
        /// <param name="lhs">Left operand.<br/>左操作数。</param>
        /// <param name="rhs">Right operand.<br/>右操作数。</param>
        /// <returns>True if both components match.<br/>若两个分量均相等则为 true。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(V2Int lhs, V2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        /// <summary>
        /// Inequality comparison for two vectors.
        /// 比较两个向量是否不相等。
        /// </summary>
        /// <param name="lhs">Left operand.<br/>左操作数。</param>
        /// <param name="rhs">Right operand.<br/>右操作数。</param>
        /// <returns>True if any component differs.<br/>若任一分量不同则为 true。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(V2Int lhs, V2Int rhs) => !(lhs == rhs);
    }
}
