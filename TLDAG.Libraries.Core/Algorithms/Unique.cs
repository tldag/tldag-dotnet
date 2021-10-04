using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Libraries.Core.Algorithms.ArrayUtils;
using static TLDAG.Libraries.Core.Algorithms.QuickSort;

namespace TLDAG.Libraries.Core.Algorithms
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

        public static int[] UniqueInts(int[] values, bool copy)
        {
            throw new NotImplementedException();
        }
        
        public static char[] UniqueChars(char[] values, bool copy)
        {
            throw new NotImplementedException();
        }

        public static string[] UniqueStrings(string[] values, bool copy, IComparer<string>? comparer = null)
            => UniqueValues(values, copy, comparer ?? StringComparer.Ordinal);

        public static T[] UniqueValues<T>(T[] values, bool copy, IComparer<T> comparer)
        {
            values = copy ? Copy(values) : values;

            Sort(values, comparer);

            throw new NotImplementedException();
        }
    }
}
