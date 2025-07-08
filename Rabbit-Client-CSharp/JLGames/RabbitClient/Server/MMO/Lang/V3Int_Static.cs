using System;
using System.Runtime.CompilerServices;
using JLGames.Infra.Mathx;

namespace JLGames.RabbitClient.Server.MMO
{
    public partial struct V3Int
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

        private static readonly V3Int s_Zero = new V3Int(0, 0, 0);
        private static readonly V3Int s_One = new V3Int(s_MagnitudeValue, s_MagnitudeValue, s_MagnitudeValue);
        private static readonly V3Int s_Up = new V3Int(0, s_MagnitudeValue, 0);
        private static readonly V3Int s_Down = new V3Int(0, -s_MagnitudeValue, 0);
        private static readonly V3Int s_Left = new V3Int(-s_MagnitudeValue, 0, 0);
        private static readonly V3Int s_Right = new V3Int(s_MagnitudeValue, 0, 0);
        private static readonly V3Int s_Forward = new V3Int(0, 0, s_MagnitudeValue);
        private static readonly V3Int s_Back = new V3Int(0, 0, -s_MagnitudeValue);

        public static V3Int zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Zero;
        }

        public static V3Int one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_One;
        }

        public static V3Int forward
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Forward;
        }

        public static V3Int back
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Back;
        }

        public static V3Int up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Up;
        }

        public static V3Int down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Down;
        }

        public static V3Int left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Left;
        }

        public static V3Int right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V3Int.s_Right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int Min(V3Int lhs, V3Int rhs) =>
            new V3Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int Max(V3Int lhs, V3Int rhs) =>
            new V3Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int Lerp(V3Int a, V3Int b, float t)
        {
            t = MathUtil.Clamp01(t);
            return new V3Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t), (int)(a.z + (b.z - a.z) * t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int LerpUnclamped(V3Int a, V3Int b, float t) =>
            new V3Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t), (int)(a.z + (b.z - a.z) * t));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator +(V3Int a, V3Int b) => new V3Int(a.x + b.x, a.y + b.y, a.z + b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator -(V3Int a, V3Int b) => new V3Int(a.x - b.x, a.y - b.y, a.z - b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator -(V3Int a) => new V3Int(-a.x, -a.y, -a.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator *(V3Int a, int b) => new V3Int(a.x * b, a.y * b, a.z * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator *(int b, V3Int a) => new V3Int(a.x * b, a.y * b, a.z * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3Int operator /(V3Int a, int b) => new V3Int(a.x / b, a.y / b, a.z / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(V3Int lhs, V3Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(V3Int lhs, V3Int rhs) => !(lhs == rhs);
    }
}