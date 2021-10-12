using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Algorithms.Algorithms;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Algorithms
{
    public static class BinarySearch
    {
        public static int Search(int[] values, int value)
            { throw NotYetImplemented(); }

        public static int Search(uint[] values, int value)
            { throw NotYetImplemented(); }

        public static int Search(char[] values, char value)
            { throw NotYetImplemented(); }

        public static int Search(string[] values, string value, IComparer<string>? comparer = null)
            => Search<string>(values, value, comparer ?? OrdinalStringComparer);

        public static int Search<T>(T[] values, T value, IComparer<T> comparer)
            => Search(values, value, 0, values.Length, comparer.ToCompare());

        public static int Search<T>(T[] values, T value, Compare<T> compare)
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

        public static int Search(uint[] values, uint value, int first, int last)
        {
            while (first < last)
            {
                int middle = (first + last) >> 1;
                uint candidate = values[middle];
                int result = value < candidate ? -1 : (value > candidate ? 1 : 0);

                if (result == 0) return middle;

                if (result < 0) last = middle;
                else first = middle + 1;
            }

            return first;
        }

        public static int Search(char[] values, int value, int first, int last)
        {
            while (first < last)
            {
                int middle = (first + last) >> 1;
                char candidate = values[middle];
                int result = value < candidate ? -1 : (value > candidate ? 1 : 0);

                if (result == 0) return middle;

                if (result < 0) last = middle;
                else first = middle + 1;
            }

            return first;
        }

        public static int Search<T>(T[] values, T value, int first, int last, IComparer<T> comparer)
            => Search(values, value, first, last, comparer.ToCompare());

        public static int Search<T>(T[] values, T value, int first, int last, Compare<T> compare)
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
