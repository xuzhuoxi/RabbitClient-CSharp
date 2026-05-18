using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace JLGames.RabbitClient.Server.MMO
{
    /// <summary>
    /// A two-dimensional integer vector (x, y).
    /// 由整数构成的二维向量 (x、y)。
    /// </summary>
    public partial struct V2Int : IEquatable<V2Int>, IFormattable
    {
        private int m_X;
        private int m_Y;

        /// <summary>
        /// X-axis component following common math and graphics conventions.
        /// X 轴分量，遵循常用的数学与图形学命名习惯。
        /// </summary>
        public int x
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.m_X;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.m_X = value;
        }

        /// <summary>
        /// Y-axis component following common math and graphics conventions.
        /// Y 轴分量，遵循常用的数学与图形学命名习惯。
        /// </summary>
        public int y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.m_Y;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.m_Y = value;
        }

        /// <summary>
        /// Gets or sets a component by zero-based index (0=x, 1=y).
        /// 按从零开始的索引获取或设置分量（0=x，1=y）。
        /// </summary>
        /// <param name="index">Component index (0 or 1).<br/>分量索引（0 或 1）。</param>
        /// <returns>The indexed component.<br/>指定索引对应的分量值。</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when index is outside 0..1.<br/>当索引不在 0..1 范围内时抛出。</exception>
        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch (index)
                {
                    case 0:
                        return this.m_X;
                    case 1:
                        return this.m_Y;
                    default:
                        throw new IndexOutOfRangeException("Invalid V2Int index!");
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch (index)
                {
                    case 0:
                        this.m_X = value;
                        break;
                    case 1:
                        this.m_Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid V2Int index!");
                }
            }
        }

        /// <summary>
        /// Constructs a vector with the specified integer components.
        /// 使用指定的整数分量构造向量。
        /// </summary>
        /// <param name="x">The x-component.<br/>x 分量。</param>
        /// <param name="y">The y-component.<br/>y 分量。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V2Int(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }

        /// <summary>
        /// Assigns both components in place.
        /// 原地同时写入两个分量。
        /// </summary>
        /// <param name="newX">The new x-component.<br/>新的 x 分量。</param>
        /// <param name="newY">The new y-component.<br/>新的 y 分量。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int newX, int newY)
        {
            this.m_X = newX;
            this.m_Y = newY;
        }

        /// <summary>
        /// Returns a hash code for this vector.
        /// 返回该向量的哈希码。
        /// </summary>
        /// <returns>A 32-bit hash code.<br/>32 位哈希码。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int num1 = this.x;
            int hashCode = num1.GetHashCode();
            num1 = this.y;
            int num2 = num1.GetHashCode() << 2;
            return hashCode ^ num2;
        }

        /// <summary>
        /// Determines whether the argument is a <see cref="V2Int"/> equal to this instance.
        /// 判断参数是否为与该实例相等的 <see cref="V2Int"/>。
        /// </summary>
        /// <param name="other">Another object.<br/>另一对象。</param>
        /// <returns>True when <paramref name="other"/> is a matching <see cref="V2Int"/>.<br/>当 <paramref name="other"/> 为相同值的 <see cref="V2Int"/> 时为 true。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is V2Int other1 && this.Equals(other1);

        /// <summary>
        /// Determines whether another vector equals this instance component-wise.
        /// 判断是否另一个向量与各分量与该实例完全一致。
        /// </summary>
        /// <param name="other">The other vector.<br/>另一向量。</param>
        /// <returns>True if all components match.<br/>若全部分量一致则为 true。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(V2Int other) => this == other;

        /// <summary>
        /// Returns the default textual representation using invariant numeric formatting internally.
        /// 使用内部的固定文化数字格式返回默认文本表示。
        /// </summary>
        /// <returns>A formatted parenthesized string.<br/>带括号的格式化字符串。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => this.ToString((string)null, (IFormatProvider)null);

        /// <summary>
        /// Returns a string representation formatted with <paramref name="format"/>.
        /// 使用 <paramref name="format"/> 格式化并返回字符串表示。
        /// </summary>
        /// <param name="format">Numeric format applied to components.<br/>应用于各分量的数字格式。</param>
        /// <returns>The formatted representation.<br/>格式化后的字符串。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format) => this.ToString(format, (IFormatProvider)null);

        /// <summary>
        /// Returns a string representation formatted with caller-provided patterns and culture.
        /// 使用调用方指定的格式与文化信息返回字符串表示。
        /// </summary>
        /// <param name="format">Numeric format; when null or empty a default format is chosen internally.<br/>数字格式；若为空则在内部选取默认格式。</param>
        /// <param name="formatProvider">Culture-specific formatting.<br/>区域性格式提供程序。</param>
        /// <returns>Parenthesized, comma-separated component text.<br/>带括号并以逗号分隔的分量文本。</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
                formatProvider = (IFormatProvider)CultureInfo.InvariantCulture.NumberFormat;
            return $"({m_X.ToString(format, formatProvider)}, {m_Y.ToString(format, formatProvider)})";
        }
    }
}
