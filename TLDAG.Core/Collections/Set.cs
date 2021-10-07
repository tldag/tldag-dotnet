using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using static TLDAG.Core.Algorithms.Algorithms;
using static TLDAG.Core.Algorithms.BinarySearch;
using static TLDAG.Core.Algorithms.Unique;

namespace TLDAG.Core.Collections
{
    public abstract partial class ImmutableSet<T> where T : notnull
    {
        protected readonly T[] values;
        private int? hashCode;

        public ImmutableSet(T[] values) { this.values = values; }

        public bool Contains(T value)
        {
            int insertPos = GetInsertPos(value);

            return insertPos != values.Length && Compare(values[insertPos], value) == 0;
        }

        public bool ContainsAny(IEnumerable<T> candidates)
            => candidates.Where(Contains).Any();

        public bool ContainsAll(IEnumerable<T> candidates)
            => candidates.Where(Contains).Count() == candidates.Count();

        public override int GetHashCode() => hashCode ??= ComputeHashCode();

        private int ComputeHashCode()
        {
            int hash = 0;

            for (int i = 0, n = values.Length; i < n; ++i)
            {
                hash <<= 1;
                hash += GetHashCode(values[i]);
            }

            return hash;
        }

        protected abstract int GetHashCode(T value);
        protected abstract int Compare(T v1, T v2);
        protected abstract int GetInsertPos(T value);
    }

    public partial class IntSet : ImmutableSet<int>
    {
        public IntSet(IEnumerable<int> values) : base(UniqueInts(values)) { }
        public IntSet(IEnumerable<int> values, int value) : this(values.Append(value)) { }
        public IntSet(IEnumerable<int> v1, IEnumerable<int> v2) : this(v1.Concat(v2)) { }

        public override int GetHashCode() => throw new NotImplementedException();
        public override bool Equals(object? obj) => throw new NotImplementedException();

        protected override int GetHashCode(int value) => value;
        protected override int Compare(int v1, int v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(int value) => Search(values, value, 0, values.Length);

        public static readonly IntSet Empty = new(Array.Empty<int>());
    }

    public partial class CharSet : ImmutableSet<char>
    {
        public CharSet(IEnumerable<char> values) : base(UniqueChars(values)) { }

        public override int GetHashCode() => throw new NotImplementedException();
        public override bool Equals(object? obj) => throw new NotImplementedException();

        protected override int GetHashCode(char value) => value;
        protected override int Compare(char v1, char v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(char value) => Search(values, value, 0, values.Length);

        public static readonly CharSet Empty = new(Array.Empty<char>());
    }

    public partial class ValueSet<T> : ImmutableSet<T> where T : notnull
    {
        private readonly Compare<T> compare;

        public ValueSet(IEnumerable<T> values, IComparer<T> comparer)
            : this(values, comparer.ToCompare()) { }

        public ValueSet(IEnumerable<T> values, Compare<T> compare)
            : base(UniqueValues(values, compare)) { this.compare = compare; }

        protected override int GetHashCode(T value) => value.GetHashCode();
        protected override int Compare(T v1, T v2) => compare(v1, v2);
        protected override int GetInsertPos(T value) => Search(values, value, 0, values.Length, compare);

        public static ValueSet<U> Empty<U>(IComparer<U> comparer) where U : notnull
            => throw new NotImplementedException();
        public static ValueSet<U> Empty<U>(Func<U, U, int> compare) where U : notnull
            => throw new NotImplementedException();
    }

    public partial class StringSet : ValueSet<string>
    {
        public StringSet(IEnumerable<string> values, IComparer<string>? comparer = null)
            : base(values, comparer ?? OrdinalStringComparer) { }

        public StringSet(IEnumerable<string> values, Compare<string> compare)
            : base(values, compare) { }

        public override int GetHashCode() => throw new NotImplementedException();
        public override bool Equals(object? obj) => throw new NotImplementedException();

        public static StringSet Empty(IComparer<string>? comparer = null) => new(Array.Empty<string>(), comparer);
        public static StringSet Empty(Compare<string> compare) => new(Array.Empty<string>(), compare);
    }
}