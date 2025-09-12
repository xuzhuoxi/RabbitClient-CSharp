using System;
using System.Runtime.CompilerServices;
using JLGames.Infra.Mathx;

namespace JLGames.RabbitClient.Server.MMO
{
    public partial struct V2Int
    {
        private static uint s_MagnitudeLevel = 4;
        private static int s_MagnitudeValue = 10000;

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

        public static int MagnitudeValue => s_MagnitudeValue;

        private static readonly V2Int s_Zero = new V2Int(0, 0);
        private static readonly V2Int s_One = new V2Int(s_MagnitudeValue, s_MagnitudeValue);
        private static readonly V2Int s_Up = new V2Int(0, s_MagnitudeValue);
        private static readonly V2Int s_Down = new V2Int(0, -s_MagnitudeValue);
        private static readonly V2Int s_Left = new V2Int(-s_MagnitudeValue, 0);
        private static readonly V2Int s_Right = new V2Int(s_MagnitudeValue, 0);

        public static V2Int zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Zero;
        }

        public static V2Int one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_One;
        }

        public static V2Int up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Up;
        }

        public static V2Int down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Down;
        }

        public static V2Int left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Left;
        }

        public static V2Int right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V2Int.s_Right;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Min(V2Int lhs, V2Int rhs) =>
            new V2Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Max(V2Int lhs, V2Int rhs) =>
            new V2Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int Lerp(V2Int a, V2Int b, float t)
        {
            t = MathUtil.Clamp01(t);
            return new V2Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int LerpUnclamped(V2Int a, V2Int b, float t) =>
            new V2Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator +(V2Int a, V2Int b) => new V2Int(a.x + b.x, a.y + b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator -(V2Int a, V2Int b) => new V2Int(a.x - b.x, a.y - b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator -(V2Int a) => new V2Int(-a.x, -a.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator *(V2Int a, int b) => new V2Int(a.x * b, a.y * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator *(int b, V2Int a) => new V2Int(a.x * b, a.y * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2Int operator /(V2Int a, int b) => new V2Int(a.x / b, a.y / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(V2Int lhs, V2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(V2Int lhs, V2Int rhs) => !(lhs == rhs);
    }
}