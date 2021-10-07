using System.Security.Cryptography;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Cryptography
{
    public static class Randoms
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static byte[] Bytes(int count) => Bytes(new byte[count]);
        public static byte[] Bytes(byte[] bytes) { rng.GetBytes(bytes); return bytes; }
        public static byte[] Bytes(byte[] bytes, int offset, int count) { rng.GetBytes(bytes, offset, count); return bytes; }

        public static int NextInt(int min, int max) => throw NotYetImplemented(nameof(NextInt));
    }
}
