using System.Security.Cryptography;
using static System.Math;
using static TLDAG.Core.Algorithms.Maths;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.Cryptography
{
    public static class Randoms
    {
        private const double DoubleStep = 1.0 / 0x7fffffffffffffffL;
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static byte[] Bytes(int count) => Bytes(new byte[count]);
        public static byte[] Bytes(byte[] bytes) { rng.GetBytes(bytes); return bytes; }
        public static byte[] Bytes(byte[] bytes, int offset, int count) { rng.GetBytes(bytes, offset, count); return bytes; }

        public static ushort NextUShort(ushort min, ushort max) => Interpolate(min, max, NextDouble());
        public static ushort NextUShort() => BytesToUShort(Bytes(4));

        public static int NextInt(int min, int max) => Interpolate(min, max, NextDouble());
        public static int NextInt() => BytesToInt(Bytes(4));

        public static double NextDouble()
        {
            byte[] bytes = Bytes(sizeof(long));
            long factor = Abs(BytesToLong(bytes)) >> 1;

            return DoubleStep * factor * 2.0;
        }
    }
}
