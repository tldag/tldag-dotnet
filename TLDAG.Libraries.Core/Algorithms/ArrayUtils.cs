using System;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class ArrayUtils
    {
        public static T[] Copy<T>(T[] values)
        {
            int count = values.Length;

            if (count == 0) return Array.Empty<T>();

            T[] copy = new T[values.Length];

            Array.Copy(values, copy, count);

            return copy;
        }
    }
}
