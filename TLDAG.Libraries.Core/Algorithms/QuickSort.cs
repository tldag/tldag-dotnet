using System;
using System.Collections.Generic;
using static System.Math;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class QuickSort
    {
        public static void Sort(byte[] values) { Sort(values, 0, values.Length); }
        public static void Sort(int[] values) { Sort(values, 0, values.Length); }
        public static void Sort<T>(T[] values, IComparer<T> comparer) { Sort(values, 0, values.Length, comparer); }

        public static void Sort(byte[] values, int offset, int count)
        {
            offset = Max(0, offset); count = Min(values.Length - offset, count);

            if (count < 2) return;
            if (count == 2) { Sort2(values, offset); return; }
            if (count == 3) { Sort3(values, offset); return; }
        }

        public static void Sort(int[] values, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public static void Sort<T>(T[] values, int offset, int count, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        private static void Sort2(byte[] values, int offset0)
        {
            int offset1 = offset0 + 1;
            byte v0 = values[offset0], v1 = values[offset1];

            if (v0 <= v1) return;
            values[offset0] = v1; values[offset1] = v0;
        }

        private static void Sort3(byte[] values, int offset0)
        {
            int offset1 = offset0 + 1, offset2 = offset0 + 2;
            byte v0 = values[offset0], v1 = values[offset1], v2 = values[offset2];

            if (v0 <= v1) { if (v1 <= v2) return; values[offset1] = v2; values[offset2] = v1; }
            else
            {
                if (v1 <= v2)
                {
                    if (v0 <= v2) { values[offset0] = v1; values[offset1] = v0; }
                    else { values[offset0] = v1; values[offset1] = v2; values[offset2] = v0; }
                }
                else { values[offset0] = v2; values[offset2] = v0; }
            }
        }
    }
}
