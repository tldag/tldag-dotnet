using System;
using System.Collections.Generic;

namespace TLDAG.Core.Algorithms
{
    public delegate int Compare<T>(T a, T b);

    public static class Algorithms
    {
        public static IComparer<string> OrdinalStringComparer => StringComparer.Ordinal;

        public static Compare<T> ToCompare<T>(this IComparer<T> comparer)
            => (a, b) => comparer.Compare(a, b);

        public static Compare<T> GetCompare<T>() where T : notnull, IComparable<T>
            => (a, b) => a.CompareTo(b);
    }
}
