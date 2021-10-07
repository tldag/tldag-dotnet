using System;
using System.Collections.Generic;

namespace TLDAG.Core.Collections
{
    public static class CollectionExtensions
    {
        public static Func<T, T, int> ToFunc<T>(this IComparer<T> comparer)
            => (a, b) => comparer.Compare(a, b);
    }
}
