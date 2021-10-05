using System;

namespace TLDAG.Libraries.Core.Collections
{
    public partial class IntSet : IEquatable<IntSet>, IComparable<IntSet>
    {
        public int CompareTo(IntSet? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IntSet? other)
        {
            throw new NotImplementedException();
        }
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
