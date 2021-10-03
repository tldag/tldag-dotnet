using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.Collections
{
    public sealed class IntSet : IReadOnlyList<int>, IEquatable<IntSet>, IComparable<IntSet>
    {
        private readonly int[] values;
        private int? hashCode = null;

        public int Count => values.Length;

        public int this[int index] => values[index];

        public readonly static IntSet Empty = new();

        public IntSet(params int[] values)
        {
            this.values = Unique(Copy(values));
        }

        public IntSet(IEnumerable<int> values)
        {
            this.values = Unique(Copy(values));
        }

        private IntSet(int[] values, bool prepared)
        {
            if (!prepared) throw new InvalidOperationException();

            this.values = values;
        }

        public IEnumerator<int> GetEnumerator()
            => GetValuesEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetValuesEnumerator();

        public override int GetHashCode()
            => hashCode ??= CalculateHashCode();

        public override bool Equals(object? obj)
            => EqualsIntSet(obj as IntSet);

        public bool Equals(IntSet? other)
            => EqualsIntSet(other);

        public override string ToString()
        {
            int count = Math.Min(10, values.Length);
            string content = string.Join(", ", values.Take(count).Select(i => i.ToString()));

            if (count < values.Length) content += ", ...";

            return content;
        }

        public int CompareTo(IntSet? other)
        {
            if (other == null) return values.Length;

            int[] v1 = values, v2 = other.values;
            int n1 = v1.Length, n2 = v2.Length;
            int i1 = 0, i2 = 0;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; }
                else if (c1 < c2) { return -1; }
                else { return 1; }
            }

            return n1 - n2;
        }

        public bool Contains(int value)
        {
            int pos = GetInsertPosition(values, value);

            return pos < values.Length && values[pos] == value;
        }

        public bool ContainsAny(params int[] values)
            => ContainsAny(values.AsEnumerable());

        public bool ContainsAny(IEnumerable<int> values)
        {
            foreach (int value in values)
            {
                if (Contains(value)) return true;
            }

            return false;
        }

        public bool ContainsAny(IntSet other)
        {
            int[] v1 = values, v2 = other.values;
            int i1 = 0, n1 = v1.Length;
            int i2 = 0, n2 = v2.Length;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) return true;

                if (c1 < c2) { ++i1; }
                else { ++i2; }
            }

            return i2 < n2;
        }

        public bool ContainsAll(params int[] values)
            => ContainsAll(values.AsEnumerable());

        public bool ContainsAll(IEnumerable<int> values)
        {
            foreach (int value in values)
            {
                if (!Contains(value)) return false;
            }

            return true;
        }

        public bool ContainsAll(IntSet other)
        {
            int[] v1 = values, v2 = other.values;
            int i1 = 0, n1 = v1.Length;
            int i2 = 0, n2 = v2.Length;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; }
                else if (c1 < c2) { ++i1; }
                else { return false; }
            }

            return i2 == n2;
        }

        public static bool operator ==(IntSet? set1, IntSet? set2)
            => set1 is null ? set2 is null : set1.EqualsIntSet(set2);

        public static bool operator !=(IntSet? set1, IntSet? set2)
            => !(set1 == set2);

        public static IntSet operator +(IntSet summand1, int summand2)
        {
            int[] oldValues = summand1.values;
            int oldCount = oldValues.Length;

            if (oldCount == 0) return new(summand2);

            int insertPos = GetInsertPosition(oldValues, summand2);

            if (insertPos < oldCount && oldValues[insertPos] == summand2) return summand1;

            int newCount = oldCount + 1;
            int[] newValues = new int[newCount];

            Array.Copy(oldValues, 0, newValues, 0, insertPos);
            newValues[insertPos] = summand2;
            Array.Copy(oldValues, insertPos, newValues, insertPos + 1, oldCount - insertPos);

            return new(newValues, true);
        }

        public static IntSet operator +(int summand1, IntSet summand2)
            => summand2 + summand1;

        public static IntSet operator +(IntSet summand1, IntSet summand2)
        {
            int[] v1 = summand1.values, v2 = summand2.values;
            int n1 = v1.Length, n2 = v2.Length;

            if (n1 == 0) return summand2;
            if (n2 == 0) return summand1;

            int count = UniqueCount(v1, v2);
            int[] result = new int[count];
            int i1 = 0, i2 = 0, j = 0;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2], v;

                if (c1 == c2) { v = c1; ++i1; ++i2; }
                else if (c1 < c2) { v = c1; ++i1; }
                else { v = c2; ++i2; }

                result[j] = v; ++j;
            }

            Array.Copy(v1, i1, result, j, n1 - i1);
            j += n1 - i1;
            Array.Copy(v2, i2, result, j, n2 - i2);

            return new(result, true);
        }

        public static IntSet operator -(IntSet minuend, int sutrahend)
        {
            int[] values = minuend.values;
            int deletePos = GetInsertPosition(values, sutrahend);
            int oldCount = values.Length;

            if (deletePos < oldCount && values[deletePos] != sutrahend) return minuend;
            if (deletePos == oldCount) return minuend;

            int[] result = new int[oldCount - 1];

            Array.Copy(values, 0, result, 0, deletePos);
            Array.Copy(values, deletePos + 1, result, deletePos, oldCount - deletePos - 1);

            return new(result, true);
        }

        public static IntSet operator -(IntSet minuend, IntSet sutrahend)
        {
            int[] v1 = minuend.values, v2 = sutrahend.values;
            int n1 = v1.Length, n2 = v2.Length;

            if (n1 == 0 || n2 == 0) return minuend;

            int i1 = 0, i2 = 0, count = 0;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; }
                else if (c1 < c2) { ++count; ++i1; }
                else { ++i2; }
            }

            count += n1 - i1; i1 = 0; i2 = 0;

            int[] result = new int[count];
            int j = 0;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; }
                else if (c1 < c2) { result[j] = c1; ++j; ++i1; }
                else { ++i2; }
            }

            Array.Copy(v1, i1, result, j, n1 - i1);

            return new(result, true);
        }

        public static IntSet operator *(IntSet set1, IntSet set2)
        {
            int[] v1 = set1.values, v2 = set2.values;
            int n1 = v1.Length, n2 = v2.Length;

            if (n1 == 0) return set1;
            if (n2 == 0) return set2;

            int i1 = 0, i2 = 0, count = 0;

            while (i1 < n1 && i2 < n2)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; ++count; }
                else if (c1 < c2) { ++i1; }
                else { ++i2; }
            }

            int[] result = new int[count];
            int j = 0;

            i1 = 0; i2 = 0;

            while (i1 < n1 && i2 < n2 && j < count)
            {
                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; result[j] = c1; ++j; }
                else if (c1 < c2) { ++i1; }
                else { ++i2; }
            }

            return new(result, true);
        }

        public void Save(Stream stream)
        {
            IntStream output = new(stream);

            output.Write(Count);
            output.Write(values);
        }

        public static IntSet Load(Stream stream)
        {
            IntStream input = new(stream);
            int count = input.Read();
            int[] values = new int[count];

            return new(values, true);
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

        private static int UniqueCount(int[] v1, int[] v2)
        {
            int count = 0;
            int i1 = 0, n1 = v1.Length;
            int i2 = 0, n2 = v2.Length;

            while (i1 < n1 && i2 < n2)
            {
                ++count;

                int c1 = v1[i1], c2 = v2[i2];

                if (c1 == c2) { ++i1; ++i2; }
                else if (c1 < c2) { ++i1; }
                else { ++i2; }
            }

            return count + (n1 - i1) + (n2 - i2);
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
            int v0 = values[offset], v1 = values[offset1], v2 = values[offset2];

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
                int v1 = values[i1], v2 = values[i2];

                if (v1 == v2) { ++i2; continue; }

                if (v1 <= v2) { ++i1; }
                else { values[i1] = v2; values[i2] = v1; }
            }
        }

        private static int[] Copy(int[] values)
        {
            if (values.Length == 0) return Array.Empty<int>();
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

            int left = 0, right = count;

            while (left < right)
            {
                int middle = (left + right) / 2;
                int candidate = values[middle];

                if (value == candidate) return middle;

                if (value < candidate) { right = middle; }
                else { left = middle + 1; }
            }

            return left;
        }

        private IEnumerator<int> GetValuesEnumerator()
            => values.AsEnumerable().GetEnumerator();

        private bool EqualsIntSet(IntSet? other)
        {
            if (other is null) return false;
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
