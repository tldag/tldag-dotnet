using System.Collections.Generic;
using TLDAG.Core.Collections.Internal;

namespace TLDAG.Core.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source) where T : notnull
            => new NotNulls<T>(source);
    }
}