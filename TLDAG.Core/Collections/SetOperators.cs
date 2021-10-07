using System;

namespace TLDAG.Core.Collections
{
    public partial class IntSet
    {
        public static bool operator ==(IntSet a, IntSet b) => throw new NotImplementedException();
        public static bool operator !=(IntSet a, IntSet b) => throw new NotImplementedException();
        public static bool operator <(IntSet a, IntSet b) => throw new NotImplementedException();
        public static bool operator >(IntSet a, IntSet b) => throw new NotImplementedException();

        public static IntSet operator +(IntSet a, IntSet b) => new(a, b);
        public static IntSet operator +(IntSet a, int b) => new(a, b);
        public static IntSet operator +(int a, IntSet b) => new(b, a);
        public static IntSet operator -(IntSet a, IntSet b) => throw new NotImplementedException();
        public static IntSet operator -(IntSet a, int b) => throw new NotImplementedException();

    }

    public partial class CharSet
    {
        public static bool operator ==(CharSet a, CharSet b) => throw new NotImplementedException();
        public static bool operator !=(CharSet a, CharSet b) => throw new NotImplementedException();
        public static bool operator <(CharSet a, CharSet b) => throw new NotImplementedException();
        public static bool operator >(CharSet a, CharSet b) => throw new NotImplementedException();

        public static IntSet operator +(CharSet a, CharSet b) => throw new NotImplementedException();
        public static IntSet operator +(CharSet a, char b) => throw new NotImplementedException();
        public static IntSet operator +(char a, CharSet b) => throw new NotImplementedException();
        public static IntSet operator -(CharSet a, CharSet b) => throw new NotImplementedException();
        public static IntSet operator -(CharSet a, char b) => throw new NotImplementedException();

    }

    public partial class StringSet
    {
        public static bool operator ==(StringSet a, StringSet b) => throw new NotImplementedException();
        public static bool operator !=(StringSet a, StringSet b) => throw new NotImplementedException();
        public static bool operator <(StringSet a, StringSet b) => throw new NotImplementedException();
        public static bool operator >(StringSet a, StringSet b) => throw new NotImplementedException();

        public static IntSet operator +(StringSet a, StringSet b) => throw new NotImplementedException();
        public static IntSet operator +(StringSet a, string b) => throw new NotImplementedException();
        public static IntSet operator +(string a, StringSet b) => throw new NotImplementedException();
        public static IntSet operator -(StringSet a, StringSet b) => throw new NotImplementedException();
        public static IntSet operator -(StringSet a, string b) => throw new NotImplementedException();

    }
}
