using System;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Algorithms
{
    public static class Arrays
    {
        public static T[] Copy<T>(T[] values)
        {
            int count = values.Length;

            if (count == 0) return Array.Empty<T>();

            T[] copy = new T[values.Length];

            Array.Copy(values, copy, count);

            return copy;
        }

        public static void Move<T>(T[] values, int srcPos, int dstPos, int count)
        {
            if (srcPos == dstPos || count == 0) return;

            if (dstPos > srcPos) MoveRight(values, srcPos, dstPos, count);
            else MoveLeft(values, srcPos, dstPos, count);
        }

        private static void MoveRight<T>(T[] values, int srcPos, int dstPos, int count)
        {
            int i = srcPos + count - 1, j = dstPos + count - 1;

            while (count > 0) { values[j--] = values[i--]; --count; }
        }

        private static void MoveLeft<T>(T[] values, int srcPos, int dstPos, int count)
        {
            throw NotYetImplemented();
        }

        public static T[] Resize<T>(T[] values, int newSize)
        {
            T[] result = new T[newSize];
            int copyCount = Math.Min(values.Length, newSize);

            Array.Copy(values, result, copyCount);

            return result;
        }
    }
}
