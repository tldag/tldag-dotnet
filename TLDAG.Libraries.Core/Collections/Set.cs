using System;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Libraries.Core.Algorithms.Algorithms;
using static TLDAG.Libraries.Core.Algorithms.BinarySearch;
using static TLDAG.Libraries.Core.Algorithms.Unique;

namespace TLDAG.Libraries.Core.Collections
{
    public abstract partial class ImmutableSet<T> where T : notnull
    {
        protected readonly T[] values;

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

        protected abstract int Compare(T v1, T v2);
        protected abstract int GetInsertPos(T value);
    }

    public partial class IntSet : ImmutableSet<int>
    {
        public IntSet(IEnumerable<int> values) : base(UniqueInts(values)) { }

        protected override int Compare(int v1, int v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(int value) => Search(values, value);

        public static readonly IntSet Empty = new(Array.Empty<int>());
    }

    public partial class CharSet : ImmutableSet<char>
    {
        public CharSet(IEnumerable<char> values) : base(UniqueChars(values)) { }

        protected override int Compare(char v1, char v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(char value) => Search(values, value);

        public static readonly CharSet Empty = new(Array.Empty<char>());
    }

    public partial class ValueSet<T> : ImmutableSet<T> where T : notnull
    {
        public readonly IComparer<T> Comparer;

        public ValueSet(IEnumerable<T> values, IComparer<T> comparer)
            : base(UniqueValues(values, comparer))
        {
            Comparer = comparer;
        }

        protected override int Compare(T v1, T v2) => Comparer.Compare(v1, v2);
        protected override int GetInsertPos(T value) => Search(values, value, Comparer);

        public static ValueSet<U> Empty<U>(IComparer<U> comparer) where U : notnull
            => new(Array.Empty<U>(), comparer);
    }

    public partial class StringSet : ValueSet<string>
    {
        public StringSet(IEnumerable<string> values, IComparer<string>? comparer = null)
            : base(values, comparer ?? OrdinalStringComparer) { }

        protected override int Compare(string v1, string v2) => Comparer.Compare(v1, v2);
        protected override int GetInsertPos(string value) => Search(values, value, Comparer);

        public static StringSet Empty(IComparer<string>? comparer = null) => new(Array.Empty<string>(), comparer);
    }
}