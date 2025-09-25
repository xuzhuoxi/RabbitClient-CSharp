using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace JLGames.RabbitClient.Server.MMO
{
    public partial struct V2Int : IEquatable<V2Int>, IFormattable
    {
        private int m_X;
        private int m_Y;

        /// <summary>
        /// 遵循数学和图形编程的传统命名习惯
        /// 在数学和图形编程中，向量的分量通常用小写字母 x、y、z 表示。这种命名方式早已成为行业标准
        /// </summary>
        public int x
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.m_X;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.m_X = value;
        }

        /// <summary>
        /// 遵循数学和图形编程的传统命名习惯
        /// 在数学和图形编程中，向量的分量通常用小写字母 x、y、z 表示。这种命名方式早已成为行业标准
        /// </summary>
        public int y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.m_Y;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.m_Y = value;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V2Int(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int newX, int newY)
        {
            this.m_X = newX;
            this.m_Y = newY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int num1 = this.x;
            int hashCode = num1.GetHashCode();
            num1 = this.y;
            int num2 = num1.GetHashCode() << 2;
            return hashCode ^ num2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is V2Int other1 && this.Equals(other1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(V2Int other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => this.ToString((string)null, (IFormatProvider)null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format) => this.ToString(format, (IFormatProvider)null);

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