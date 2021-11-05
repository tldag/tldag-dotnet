using System;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.Core.Collections
{
    public static class Enumerables
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source) where T : notnull
            { foreach (T? value in source) if (value is not null) yield return value; }

        public static IEnumerable<T> SafeCast<T>(this IEnumerable source)
        {
            foreach (object o in source)
            {
                if (o is T t) yield return t;
            }
        }

        public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Action<T> action)
            { foreach (T t in source) action(t); return source; }
    }
}