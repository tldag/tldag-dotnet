using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Libraries.Core.Algorithms.Algorithms;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class BinarySearch
    {
        public static int Search(int[] values, int value)
            { throw new NotImplementedException(); }

        public static int Search(char[] values, char value)
            { throw new NotImplementedException(); }

        public static int Search(string[] values, string value, IComparer<string>? comparer = null)
            => Search<string>(values, value, comparer ?? DefaultStringComparer);

        public static int Search<T>(T[] values, string value, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }
    }
}
