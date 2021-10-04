using System;
using System.Collections;
using System.Collections.Generic;
using static TLDAG.Libraries.Core.Algorithms.Unique;
using static TLDAG.Libraries.Core.Algorithms.BinarySearch;
using static TLDAG.Libraries.Core.Algorithms.Algorithms;
using System.Linq;

namespace TLDAG.Libraries.Core.Collections
{
    public abstract class ImmutableSet<T> : IReadOnlyList<T> where T : notnull
    {
        protected readonly T[] values;

        public T this[int index] => values[index];
        public int Count => values.Length;

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

        public IEnumerator<T> GetEnumerator() => CreateEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => CreateEnumerator();

        protected IEnumerator<T> CreateEnumerator() => new Enumerator(this);

        protected class Enumerator : IEnumerator<T>
        {
            protected readonly ImmutableSet<T> set;
            protected int index = -1;

            public T Current => set[index];
            object IEnumerator.Current => set[index];

            public Enumerator(ImmutableSet<T> set) { this.set = set; }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { index = -1; }

            public bool MoveNext()
            {
                if (index + 1 < set.Count) { ++index; return true; }
                return false;
            }
        }
    }

    public class IntSet : ImmutableSet<int>
    {
        public static readonly IntSet Empty = new(Array.Empty<int>());

        public IntSet(IEnumerable<int> values) : base(UniqueInts(values)) { }

        protected override int Compare(int v1, int v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(int value) => Search(values, value);
    }

    public class CharSet : ImmutableSet<char>
    {
        public static readonly CharSet Empty = new(Array.Empty<char>());

        public CharSet(IEnumerable<char> values) : base(UniqueChars(values)) { }

        protected override int Compare(char v1, char v2) => v1 < v2 ? -1 : (v1 > v2 ? 1 : 0);
        protected override int GetInsertPos(char value) => Search(values, value);
    }

    public class StringSet : ImmutableSet<string>
    {
        public readonly IComparer<string> Comparer;

        public StringSet(IEnumerable<string> values, IComparer<string>? comparer = null)
            : base(UniqueStrings(values, comparer ?? DefaultStringComparer))
        {
            Comparer = comparer ?? DefaultStringComparer;
        }

        protected override int Compare(string v1, string v2) => Comparer.Compare(v1, v2);
        protected override int GetInsertPos(string value) => Search(values, value, Comparer);
    }
}