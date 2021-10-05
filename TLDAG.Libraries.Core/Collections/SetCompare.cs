using System;
using TLDAG.Libraries.Core.Algorithms;

namespace TLDAG.Libraries.Core.Collections
{
    public partial class IntSet : IEquatable<IntSet>, IComparable<IntSet>
    {
        public int CompareTo(IntSet? other)
            => other is null ? 1 : Comparing.Compare(values, other.values);

        public bool Equals(IntSet? other)
            => CompareTo(other) == 0;
    }

    public partial class CharSet : IEquatable<CharSet>, IComparable<CharSet>
    {
        public int CompareTo(CharSet? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CharSet? other)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ValueSet<T> : IEquatable<ValueSet<T>>, IComparable<ValueSet<T>>
        where T : notnull
    {
        public int CompareTo(ValueSet<T>? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ValueSet<T>? other)
        {
            throw new NotImplementedException();
        }
    }

    public partial class StringSet : IEquatable<StringSet>, IComparable<StringSet>
    {
        public int CompareTo(StringSet? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(StringSet? other)
        {
            throw new NotImplementedException();
        }
    }
}
