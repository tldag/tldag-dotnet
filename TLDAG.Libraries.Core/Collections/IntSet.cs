using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.Libraries.Core.Collections
{
    public sealed class IntSet : IReadOnlyList<int>, IEquatable<IntSet>
    {
        private readonly int[] values;
        private int? hashCode = null;

        public int Count => values.Length;

        public int this[int index] => values[index];

        public IntSet(params int[] values)
        {
            this.values = Unique(Copy(values));
        }

        public IntSet(IEnumerable<int> values)
        {
            this.values = Unique(Copy(values));
        }

        public IEnumerator<int> GetEnumerator()
            => GetValuesEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetValuesEnumerator();

        public override int GetHashCode()
            => hashCode ??= CalculateHashCode();

        public override bool Equals(object? obj)
        {
            IntSet? other = obj as IntSet;

            return EqualsIntSet(other);
        }

        public bool Equals(IntSet? other)
            => EqualsIntSet(other);

        public bool Contains(int value)
        {
            int pos = GetInsertPosition(values, value);

            return pos < values.Length && values[pos] == value;
        }

        private static int[] Unique(int[] values)
        {
            Sort(values, 0, values.Length);

            int count = UniqueCount(values);

            if (count == 0) return values;

            int[] result = new int[count];
            int current = values[0];

            result[0] = current;

            for (int i = 1, j = 1, n = values.Length; i < n; ++i)
            {
                int vi = values[i];

                if (vi > current)
                {
                    result[j] = vi;
                    ++j;
                    current = vi;
                }
            }

            return result;
        }

        private static int UniqueCount(int[] values)
        {
            if (values.Length == 0) return 0;
            if (values.Length == 1) return 1;

            int count = 1;
            int v = values[0];

            for (int i = 1, n = values.Length; i < n; ++i)
            {
                int vi = values[i];

                if (vi > v)
                {
                    ++count;
                    v = vi;
                }
            }

            return count;
        }

        private static void Sort(int[] values, int offset, int count)
        {
            if (count < 2) return;
            if (count == 2) { Sort2(values, offset); return; }
            if (count == 3) { Sort3(values, offset); return; }

            int left = offset;
            int right = left + count;
            int middle = (left + right) / 2;
            int count1 = middle - left;
            int count2 = right - middle;

            Sort(values, left, count1);
            Sort(values, middle, count2);
            Merge(values, left, count1, count2);
        }

        private static void Sort2(int[] values, int offset)
        {
            int offset1 = offset + 1;
            int v0 = values[offset];
            int v1 = values[offset1];

            if (v0 <= v1) return;

            values[offset] = v1;
            values[offset1] = v0;
        }

        private static void Sort3(int[] values, int offset)
        {
            int offset1 = offset + 1;
            int offset2 = offset + 2;
            int v0 = values[offset];
            int v1 = values[offset1];
            int v2 = values[offset2];

            if (v0 <= v1)
            {
                if (v1 <= v2) return;

                values[offset1] = v2;
                values[offset2] = v1;
            }
            else
            {
                if (v1 <= v2)
                {
                    // v0 > v1 <= v2
                    
                    if (v0 <= v2)
                    {
                        // v0 > v1 && v0 <= v2 && v1 <= v2
                        // order: v1, v0, v2
                        values[offset] = v1;
                        values[offset1] = v0;
                    }
                    else
                    {
                        // v0 > v1 && v0 > v2 && v1 <= v2
                        // order: v1, v2, v0
                        values[offset] = v1;
                        values[offset1] = v2;
                        values[offset2] = v0;
                    }
                }
                else
                {
                    // v0 > v1 && v1 > v2
                    // order: v2, v1, v0
                    values[offset] = v2;
                    values[offset2] = v0;
                }
            }
        }

        private static void Merge(int[] values, int offset1, int count1, int count2)
        {
            int i1 = offset1;
            int e1 = i1 + count1;
            int i2 = e1;
            int e2 = i2 + count2;

            while (i1 < e1 && i2 < e2)
            {
                int v1 = values[i1];
                int v2 = values[i2];

                if (v1 == v2)
                {
                    ++i2;
                    continue;
                }

                if (v1 <= v2)
                {
                    ++i1;
                }
                else
                {
                    values[i1] = v2;
                    values[i2] = v1;
                }
            }
        }

        private static int[] Copy(int[] values)
        {
            if (values.Length == 0) return new int[0];
            if (values.Length == 1) return new int[] { values[0] };
            if (values.Length == 2) return new int[] { values[0], values[1] };

            int[] result = new int[values.Length];

            Array.Copy(values, result, values.Length);

            return result;
        }

        private static int[] Copy(IEnumerable<int> values)
        {
            int count = values.Count();
            int[] result = new int[count];
            using IEnumerator<int> enumerator = values.GetEnumerator();

            for (int i = 0; i < count; ++i)
            {
                enumerator.MoveNext();
                result[i] = enumerator.Current;
            }

            return result;
        }

        private static int GetInsertPosition(int[] values, int value)
        {
            int count = values.Length;

            if (count == 0) return 0;

            int left = 0;
            int right = count;

            while (left < right)
            {
                int middle = (left + right) / 2;
                int candidate = values[middle];

                if (value == candidate) return middle;

                if (value < candidate)
                {
                    right = middle;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return left;
        }

        private IEnumerator<int> GetValuesEnumerator()
            => values.AsEnumerable().GetEnumerator();

        private bool EqualsIntSet(IntSet? other)
        {
            if (other == null) return false;
            if (values.Length != other.values.Length) return false;

            int[] otherValues = other.values;

            for (int i = 0, n = values.Length; i < n; ++i)
            {
                if (values[i] != otherValues[i]) return false;
            }

            return true;
        }

        private int CalculateHashCode()
        {
            int hash = values.Length;

            for (int i = 0, n = values.Length; i < n; ++i)
            {
                hash <<= 1;
                hash += values[i];
            }

            return hash;
        }
    }
}
