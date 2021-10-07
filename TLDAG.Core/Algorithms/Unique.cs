using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Algorithms.QuickSort;

namespace TLDAG.Core.Algorithms
{
    public static class Unique
    {
        public static int[] UniqueInts(IEnumerable<int> values)
            => values is int[] array ? UniqueInts(array, true) : UniqueInts(values.ToArray(), false);

        public static char[] UniqueChars(IEnumerable<char> values)
            => values is char[] array ? UniqueChars(array, true) : UniqueChars(values.ToArray(), false);

        public static string[] UniqueStrings(IEnumerable<string> values, IComparer<string>? comparer = null)
            => values is string[] array ? UniqueStrings(array, true) : UniqueStrings(values.ToArray(), false, comparer);

        public static T[] UniqueValues<T>(IEnumerable<T> values, IComparer<T> comparer)
            => values is T[] array ? UniqueValues(array, true, comparer) : UniqueValues(values.ToArray(), false, comparer);

        public static T[] UniqueValues<T>(IEnumerable<T> values, Func<T, T, int> compare)
            => values is T[] array ? UniqueValues(array, true, compare) : UniqueValues(values.ToArray(), false, compare);

        public static int[] UniqueInts(int[] values, bool copy)
        {
            if (values.Length == 0) return values;

            int[] sorted = copy ? Copy(values) : values;

            Sort(sorted);

            int count = UniqueIntsCount(sorted);
            int[] result = new int[count];
            int current = result[0] = sorted[0];

            for (int i = 1, j = 1; j < count; ++i)
            {
                int candidate = sorted[i];

                if (candidate > current) { result[j++] = current = candidate; }
            }

            return result;
        }
        
        public static char[] UniqueChars(char[] values, bool copy)
        {
            if (values.Length == 0) return values;

            char[] sorted = copy ? Copy(values) : values;

            Sort(sorted);

            int count = UniqueCharsCount(sorted);
            char[] result = new char[count];
            char current = result[0] = sorted[0];

            for (int i = 1, j = 1; j < count; ++i)
            {
                char candidate = sorted[i];

                if (candidate > current) { result[j++] = current = candidate; }
            }

            return result;
        }

        public static string[] UniqueStrings(string[] values, bool copy, IComparer<string>? comparer = null)
            => UniqueValues(values, copy, comparer ?? StringComparer.Ordinal);

        public static T[] UniqueValues<T>(T[] values, bool copy, IComparer<T> comparer)
            => UniqueValues(values, copy, comparer.ToFunc());

        public static T[] UniqueValues<T>(T[] values, bool copy, Func<T, T, int> compare)
        {
            if (values.Length == 0) return values;

            T[] sorted = copy ? Copy(values) : values;

            Sort(sorted, compare);

            int count = UniqueValuesCount(sorted, compare);
            T[] result = new T[count];
            T current = result[0] = sorted[0];

            for (int i = 1, j = 1; j < count; ++i)
            {
                T candidate = sorted[i];

                if (compare(candidate, current) > 0) { result[j++] = current = candidate; }
            }

            return result;
        }

        private static int UniqueIntsCount(int[] sorted)
        {
            int count = sorted.Length;

            if (count == 0) return 0;

            int result = 1, last = sorted[0];

            for (int i = 1; i < count; ++i)
            {
                int current = sorted[i];

                if (current > last) { ++result; last = current; }
            }

            return result;
        }

        private static int UniqueCharsCount(char[] sorted)
        {
            throw new NotImplementedException();
        }

        private static int UniqueValuesCount<T>(T[] sorted, Func<T, T, int> compare)
        {
            int count = sorted.Length;

            if (count == 0) return 0;

            int result = 1; T last = sorted[0];

            for (int i = 1; i < count; ++i)
            {
                T current = sorted[i];

                if (compare(current, last) > 0) { ++result; last = current; }
            }

            return result;
        }
    }
}
