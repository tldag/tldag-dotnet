using System.Collections.Generic;

namespace TLDAG.Core.Collections
{
    public static class Enumerables
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source) where T : notnull
            { foreach (T? value in source) if (value is not null) yield return value; }
    }
}