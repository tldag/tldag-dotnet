using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Conversion
{
    public static class Primitives
    {
        public static byte[] IntToBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static int BytesToInt(byte[] bytes)
        {
            bytes = BitConverter.IsLittleEndian ? bytes : CopyAndReverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        private static byte[] CopyAndReverse(byte[] value)
        {
            byte[] copy = new byte[value.Length];
            Array.Copy(value, copy, copy.Length);
            Array.Reverse(copy);
            return copy;
        }
    }
}
