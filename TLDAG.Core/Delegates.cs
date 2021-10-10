using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core
{
    public delegate int Compare<T>(T a, T b);

    public static class Delegates
    {
        public static Compare<T> ToCompare<T>(this IComparer<T> comparer)
            => (a, b) => comparer.Compare(a, b);

        public static Compare<T> GetCompare<T>() where T : notnull, IComparable<T>
            => (a, b) => a.CompareTo(b);
    }
}
