using System;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core
{
    public static class Primitives
    {
        public static readonly Compare<int> IntCompare = (a, b) => a < b ? -1 : (a > b ? 1 : 0);
        public static readonly Compare<uint> UIntCompare = (a, b) => a < b ? -1 : (a > b ? 1 : 0);
        public static readonly Compare<char> CharCompare = (a, b) => a < b ? -1 : (a > b ? 1 : 0);

        public static void UShortToBytes(ushort value, byte[] dest, int offset) => throw NotYetImplemented();

        public static ushort ToUShort(byte[] src, int offset = 0)
        {
            if (!BitConverter.IsLittleEndian) { src = SubArray(src, 0, sizeof(ushort)); offset = 0; }
            return BitConverter.ToUInt16(src, offset);
        }

        public static void IntToBytes(int value, byte[] dest, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
            Replace(dest, offset, bytes, 0, bytes.Length);
        }

        public static byte[] IntToBytes(int[] values)
        {
            byte[] result = new byte[values.Length * sizeof(int)];
            for (int i = 0, n = values.Length, j = 0; i < n; ++i, j += sizeof(int)) IntToBytes(values[i], result, j);
            return result;
        }

        public static byte[] IntToBytes(int value) => IntToBytes(new int[] { value });

        public static int ToInt(byte[] src, int offset = 0)
        {
            if (!BitConverter.IsLittleEndian)
                { src = SubArray(src, 0, sizeof(int)); Array.Reverse(src); offset = 0; }

            return BitConverter.ToInt32(src, offset);
        }

        public static void LongToBytes(long value, byte[] dest, int offset) => throw NotYetImplemented();

        public static long ToLong(byte[] src, int offset = 0)
        {
            if (!BitConverter.IsLittleEndian)
                { src = SubArray(src, 0, sizeof(ushort)); Array.Reverse(src); offset = 0; }

            return BitConverter.ToInt64(src, offset);
        }

        public static void CharToBytes(char value, byte[] dest, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
            Replace(dest, offset, bytes, 0, bytes.Length);
        }

        public static char ToChar(byte[] src, int offset = 0) => throw NotYetImplemented();

        public static byte[] ToBytes(char[] values)
        {
            byte[] result = new byte[values.Length * sizeof(char)];
            for (int i = 0, n = values.Length, j = 0; i < n; ++i, j += sizeof(char)) CharToBytes(values[i], result, j);
            return result;
        }
    }
}
