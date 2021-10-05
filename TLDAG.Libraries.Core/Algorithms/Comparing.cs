using System;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class Comparing
    {
        public static int Compare(int[] a1, int[] a2)
        {
            int n1 = a1.Length, n2 = a2.Length, result = 0;
            int n = Math.Min(n1, n2);

            for (int i = 0; i < n && result == 0; ++i)
            { int v1 = a1[i], v2 = a2[i]; result = v1 < v2 ? -1 : (v1 > v2 ? 1 : 0); }

            if (result == 0) result = n1 < n2 ? -1 : (n1 > n2 ? 1 : 0);

            return result;
        }
    }
}
