using System;
using System.Collections.Generic;
using static TLDAG.Libraries.Core.Algorithms.Algorithms;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class BinarySearch
    {
        public static int Search(int[] values, int value)
            { throw new NotImplementedException(); }

        public static int Search(char[] values, char value)
            { throw new NotImplementedException(); }

        public static int Search(string[] values, string value, IComparer<string>? comparer = null)
            => Search<string>(values, value, comparer ?? OrdinalStringComparer);

        public static int Search<T>(T[] values, T value, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public static int Search(int[] values, int value, int first, int last)
        {
            while (first < last)
            {
                int middle = (first + last) >> 1;
                int candidate = values[middle];
                int result = value < candidate ? -1 : (value > candidate ? 1 : 0);

                if (result == 0) return middle;

                if (result < 0) last = middle;
                else first = middle + 1;
            }

            return first;
        }

        public static int Search<T>(T[] values, T value, int first, int last, IComparer<T> comparer)
        {
            while (first < last)
            {
                int middle = (first + last) >> 1;
                T candidate = values[middle];
                int result = comparer.Compare(value, candidate);

                if (result == 0) return middle;

                if (result < 0) last = middle;
                else first = middle + 1;
            }

            return first;
        }
    }
}
