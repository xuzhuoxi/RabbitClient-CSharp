using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace JLGames.RabbitClient.Server.MMO
{
    public partial struct V3Int : IEquatable<V3Int>, IFormattable
    {
        private int m_X;
        private int m_Y;
        private int m_Z;

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

        /// <summary>
        /// 遵循数学和图形编程的传统命名习惯
        /// 在数学和图形编程中，向量的分量通常用小写字母 x、y、z 表示。这种命名方式早已成为行业标准
        /// </summary>
        public int z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.m_Z;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.m_Z = value;
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
                    case 2:
                        return this.m_Z;
                    default:
                        throw new IndexOutOfRangeException("Invalid V3Int index!");
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
                    case 2:
                        this.m_Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid V3Int index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3Int(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3Int(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int newX, int newY, int newZ)
        {
            this.m_X = newX;
            this.m_Y = newY;
            this.m_Z = newZ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int hashCode1 = this.m_Y.GetHashCode();
            int hashCode2 = this.m_Z.GetHashCode();
            return this.m_X.GetHashCode() ^ hashCode1 << 4 ^ hashCode1 >> 28 ^ hashCode2 >> 4 ^ hashCode2 << 28;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is V3Int other1 && this.Equals(other1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(V3Int other) => this == other;

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
            return $"({m_X.ToString(format, formatProvider)}, {m_Y.ToString(format, formatProvider)}, {m_Z.ToString(format, formatProvider)})";
        }
    }
}