using System.Security.Cryptography;
using static TLDAG.Core.Algorithms.Conversion;
using static TLDAG.Core.Algorithms.Maths;

namespace TLDAG.Core.Cryptography
{
    public static class Randoms
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static byte[] Bytes(int count) => Bytes(new byte[count]);
        public static byte[] Bytes(byte[] bytes) { rng.GetBytes(bytes); return bytes; }
        public static byte[] Bytes(byte[] bytes, int offset, int count) { rng.GetBytes(bytes, offset, count); return bytes; }

        public static int NextInt(int min, int max) => Mod(NextInt(), min, max);
        public static int NextInt() => ToInt(Bytes(4));
    }
}
