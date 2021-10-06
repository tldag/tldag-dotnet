using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.Collections
{
    public static class CollectionExtensions
    {
        public static Func<T, T, int> ToFunc<T>(this IComparer<T> comparer)
            => (a, b) => comparer.Compare(a, b);
    }
}
