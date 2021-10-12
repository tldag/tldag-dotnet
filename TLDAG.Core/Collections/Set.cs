using System;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Algorithms.Algorithms;
using static TLDAG.Core.Algorithms.BinarySearch;
using static TLDAG.Core.Algorithms.Unique;
using static TLDAG.Core.Delegates;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.Collections
{
    public abstract partial class ImmutableSet<T> : IComparable<ImmutableSet<T>>, IEquatable<ImmutableSet<T>>
        where T : notnull
    {
        protected readonly Compare<T> compare;

        protected readonly T[] values;
        private int? hashCode;

        protected ImmutableSet(Compare<T> compare, T[] values) { this.compare = compare; this.values = values; }
        protected ImmutableSet(Compare<T> compare, T value) { this.compare = compare; this.values = new T[] { value }; }

        public bool Contains(T value)
        {
            if (values.Length == 0) return false;
            if (values.Length == 1) return EqualValues(values[0], value);

            int insertPos = SearchValue(value);

            return insertPos != values.Length && EqualValues(values[insertPos], value);
        }

        public bool ContainsAny(IEnumerable<T> candidates)
            => candidates.Where(Contains).Any();

        public bool ContainsAll(IEnumerable<T> candidates)
            => candidates.Where(Contains).Count() == candidates.Count();

        public override int GetHashCode() => hashCode ??= ComputeHashCode();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        public int CompareTo(ImmutableSet<T>? other)
        {
            throw NotYetImplemented();
        }

        public bool Equals(ImmutableSet<T>? other)
        {
            throw NotYetImplemented();
        }

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

        protected virtual int SearchValue(T value) => Search(values, value, 0, values.Length, compare);
        protected virtual bool EqualValues(T a, T b) => compare(a, b) == 0;

        protected abstract int GetHashCode(T value);

    }

    public partial class IntSet : ImmutableSet<int>
    {
        public IntSet(IEnumerable<int> values) : base(IntCompare, UniqueInts(values)) { }
        public IntSet(params int[] values) : base(IntCompare, UniqueInts(values)) { }
        public IntSet(IEnumerable<int> values, int value) : this(values.Append(value)) { }
        public IntSet(IEnumerable<int> v1, IEnumerable<int> v2) : this(v1.Concat(v2)) { }
        public IntSet(int value) : base(IntCompare, value) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        protected override int SearchValue(int value) => Search(values, value, 0, values.Length);
        protected override bool EqualValues(int a, int b) => a == b;

        protected override int GetHashCode(int value) => value;

        public static readonly IntSet Empty = new();
    }

    public partial class UIntSet : ImmutableSet<uint>
    {
        public UIntSet(IEnumerable<uint> values) : base(UIntCompare, UniqueUInts(values)) { }
        public UIntSet(params uint[] values) : base(UIntCompare, UniqueUInts(values)) { }
        public UIntSet(IEnumerable<uint> values, uint value) : this(values.Append(value)) { }
        public UIntSet(IEnumerable<uint> v1, IEnumerable<uint> v2) : this(v1.Concat(v2)) { }
        public UIntSet(uint value) : base(UIntCompare, value) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        protected override int SearchValue(uint value) => Search(values, value, 0, values.Length);
        protected override bool EqualValues(uint a, uint b) => a == b;

        protected override int GetHashCode(uint value) => value.GetHashCode();

        public static readonly UIntSet Empty = new();
    }

    public partial class CharSet : ImmutableSet<char>
    {
        public CharSet(IEnumerable<char> values) : base(CharCompare, UniqueChars(values)) { }
        public CharSet(params char[] values) : base(CharCompare, UniqueChars(values)) { }
        public CharSet(IEnumerable<char> values, char value) : this(values.Append(value)) { }
        public CharSet(IEnumerable<char> v1, IEnumerable<char> v2) : this(v1.Concat(v2)) { }
        public CharSet(char value) : base(CharCompare, value) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        protected override int SearchValue(char value) => Search(values, value, 0, values.Length);
        protected override bool EqualValues(char a, char b) => a == b;

        protected override int GetHashCode(char value) => value;

        public static readonly CharSet Empty = new();
    }

    public partial class ValueSet<T> : ImmutableSet<T> where T : notnull
    {
        public ValueSet(IComparer<T> comparer, IEnumerable<T> values)
            : this(comparer.ToCompare(), values) { }

        public ValueSet(Compare<T> compare, IEnumerable<T> values)
            : base(compare, UniqueValues(values, compare)) { }

        public ValueSet(IComparer<T> comparer, IEnumerable<T> values, T value)
            : this(comparer.ToCompare(), values, value) { }

        public ValueSet(Compare<T> compare, IEnumerable<T> values, T value)
            : this(compare, values.Append(value)) { }

        public ValueSet(IComparer<T> comparer, params T[] values)
            : this(comparer.ToCompare(), values) { }

        public ValueSet(Compare<T> compare, params T[] values)
            : base(compare, UniqueValues(values, compare)) { }

        public ValueSet(IComparer<T> comparer, T value)
            : this(comparer.ToCompare(), value) { }

        public ValueSet(Compare<T> compare, T value)
            : base(compare, value) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        protected override int GetHashCode(T value) => value.GetHashCode();

        public static ValueSet<U> Empty<U>(IComparer<U> comparer) where U : notnull
            => new(comparer);

        public static ValueSet<U> Empty<U>(Compare<U> compare) where U : notnull
            => new(compare);
    }

    public partial class StringSet : ValueSet<string>
    {
        public StringSet(IEnumerable<string> values, IComparer<string>? comparer = null)
            : base(comparer ?? OrdinalStringComparer, values) { }

        public StringSet(IEnumerable<string> values, Compare<string> compare)
            : base(compare, values) { }

        public StringSet(IEnumerable<string> values, string value, IComparer<string>? comparer = null)
            : this(values.Append(value), comparer) { }

        public StringSet(IEnumerable<string> values, string value, Compare<string> compare)
            : this(values.Append(value), compare) { }

        public StringSet(string value, IComparer<string>? comparer = null)
            : base(comparer ?? OrdinalStringComparer, value) { }

        public StringSet(string value, Compare<string> compare)
            : base(compare, value) { }

        public StringSet(IComparer<string> comparer, params string[] values)
            : base(comparer, values) { }

        public StringSet(Compare<string> compare, params string[] values)
            : base(compare, values) { }

        public StringSet(params string[] values)
            : this(OrdinalStringComparer, values) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        public static readonly StringSet OrdinalEmpty = new(OrdinalStringComparer);

        public static StringSet Empty(IComparer<string>? comparer = null) => comparer == null ? OrdinalEmpty : new(comparer);
        public static StringSet Empty(Compare<string> compare) => new(compare);
    }

    public partial class SmartSet<T> : ValueSet<T> where T : notnull, IComparable<T>
    {
        public SmartSet(IEnumerable<T> values) : base(GetCompare<T>(), values) { }
        public SmartSet(IEnumerable<T> values, T value) : this(values.Append(value)) { }
        public SmartSet(T value) : base(GetCompare<T>(), value) { }
        public SmartSet(params T[] values) : base(GetCompare<T>(), values) { }

        public override int GetHashCode() => throw NotYetImplemented();
        public override bool Equals(object? obj) => throw NotYetImplemented();

        public static SmartSet<U> Empty<U>() where U : notnull, IComparable<U> => new();
    }
}