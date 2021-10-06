using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;
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
            => Search(values, value, 0, values.Length, comparer.ToFunc());

        public static int Search<T>(T[] values, T value, Func<T, T, int> compare)
            => Search(values, value, 0, values.Length, compare);

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

        public static int Search(char[] values, int value, int first, int last)
        {
            throw new NotImplementedException();
        }

        public static int Search<T>(T[] values, T value, int first, int last, IComparer<T> comparer)
            => Search(values, value, first, last, comparer.ToFunc());

        public static int Search<T>(T[] values, T value, int first, int last, Func<T, T, int> compare)
        {
            while (first < last)
            {
                int middle = (first + last) >> 1;
                T candidate = values[middle];
                int result = compare(value, candidate);

                if (result == 0) return middle;

                if (result < 0) last = middle;
                else first = middle + 1;
            }

            return first;
        }
    }
}
