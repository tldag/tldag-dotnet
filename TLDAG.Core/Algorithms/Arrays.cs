using System;

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
            throw new NotImplementedException();
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
