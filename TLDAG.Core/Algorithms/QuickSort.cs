using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static System.Math;

namespace TLDAG.Core.Algorithms
{
    public static class QuickSort
    {
        public static void Sort(byte[] values) { Sort(values, 0, values.Length); }
        public static void Sort(int[] values) { Sort(values, 0, values.Length); }
        public static void Sort(char[] values) { Sort(values, 0, values.Length); }

        public static void Sort<T>(T[] values, IComparer<T> comparer) { Sort(values, 0, values.Length, comparer); }
        public static void Sort<T>(T[] values, Compare<T> compare) { Sort(values, 0, values.Length, compare); }

        public static void Sort(byte[] values, int offset, int count)
        {
            offset = Max(0, offset); count = Min(values.Length - offset, count);

            if (count < 2) return;
            if (count == 2) { Sort2(values, offset); return; }
            if (count == 3) { Sort3(values, offset); return; }

            throw new NotImplementedException();
        }

        public static void Sort(int[] values, int offset, int count)
        {
            offset = Max(0, offset); count = Min(values.Length - offset, count);

            if (count < 2) return;
            if (count == 2) { Sort2(values, offset); return; }
            if (count == 3) { Sort3(values, offset); return; }

            int count1 = count / 2, count2 = count - count1;

            Sort(values, offset, count1);
            Sort(values, offset + count1, count2);
            Merge(values, offset, count1, count2);
        }

        public static void Sort(char[] values, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public static void Sort<T>(T[] values, int offset, int count, IComparer<T> comparer)
            => Sort(values, offset, count, comparer.ToCompare());

        public static void Sort<T>(T[] values, int offset, int count, Compare<T> compare)
        {
            offset = Max(0, offset); count = Min(values.Length - offset, count);

            if (count < 2) return;
            if (count == 2) { Sort2(values, offset, compare); return; }
            if (count == 3) { Sort3(values, offset, compare); return; }

            int count1 = count / 2, count2 = count - count1;

            Sort(values, offset, count1, compare);
            Sort(values, offset + count1, count2, compare);
            Merge(values, offset, count1, count2, compare);
        }

        private static void Sort2(byte[] values, int offset0)
        {
            int offset1 = offset0 + 1;
            byte v0 = values[offset0], v1 = values[offset1];

            if (v0 <= v1) return;
            values[offset0] = v1; values[offset1] = v0;
        }

        private static void Sort2(int[] values, int offset0)
        {
            int offset1 = offset0 + 1;
            int v0 = values[offset0], v1 = values[offset1];

            if (v0 <= v1) return;
            values[offset0] = v1; values[offset1] = v0;
        }

        private static void Sort2<T>(T[] values, int offset, Compare<T> compare)
        {
            throw new NotImplementedException();
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

        private static void Sort3(int[] values, int offset0)
        {
            int offset1 = offset0 + 1, offset2 = offset0 + 2;
            int v0 = values[offset0], v1 = values[offset1], v2 = values[offset2];

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

        private static void Sort3<T>(T[] values, int offset0, Compare<T> compare)
        {
            int offset1 = offset0 + 1, offset2 = offset0 + 2;
            T v0 = values[offset0], v1 = values[offset1], v2 = values[offset2];

            if (compare(v0, v1) <= 0) { if (compare(v1, v2) <= 0) return; values[offset1] = v2; values[offset2] = v1; }
            else
            {
                if (compare(v1, v2) <= 0)
                {
                    if (compare(v0, v2) <= 0) { values[offset0] = v1; values[offset1] = v0; }
                    else { values[offset0] = v1; values[offset1] = v2; values[offset2] = v0; }
                }
                else { values[offset0] = v2; values[offset2] = v0; }
            }
        }

        private static void Merge(int[] values, int offset, int count1, int count2)
        {
            int i1 = offset, e1 = i1 + count1, i2 = e1, e2 = i2 + count2;

            while (i1 < e1 && i2 < e2)
            {
                int v1 = values[i1], v2 = values[i2];

                if (v1 == v2) { ++i2; continue; }
                if (v1 <= v2) { ++i1; }
                else { values[i1] = v2; values[i2] = v1; }
            }
        }

        private static void Merge<T>(T[] values, int offset, int count1, int count2, Compare<T> compare)
        {
            throw new NotImplementedException();
        }
    }
}
