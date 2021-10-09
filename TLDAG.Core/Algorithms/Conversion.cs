using System;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Algorithms.Arrays;

namespace TLDAG.Core.Algorithms
{
    public static class Conversion
    {
        public static void UShortToBytes(ushort value, byte[] dest, int offset) => throw NotYetImplemented();

        public static ushort ToUShort(byte[] src, int offset = 0)
        {
            if (!BitConverter.IsLittleEndian) { src = SubArray(src, 0, sizeof(ushort)); offset = 0; }
            return BitConverter.ToUInt16(src, offset);
        }

        public static void IntToBytes(int value, byte[] dest, int offset) => throw NotYetImplemented();

        public static int ToInt(byte[] src, int offset = 0) => throw NotYetImplemented();

        public static long ToLong(byte[] src, int offset = 0)
        {
            if (!BitConverter.IsLittleEndian) { src = SubArray(src, 0, sizeof(ushort)); offset = 0; }
            return BitConverter.ToInt64(src, offset);
        }
    }
}
