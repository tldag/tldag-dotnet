using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Collections
{
    public partial class IntSet
    {
        public static bool operator ==(IntSet a, IntSet b) => throw NotYetImplemented();
        public static bool operator !=(IntSet a, IntSet b) => throw NotYetImplemented();
        public static bool operator <(IntSet a, IntSet b) => throw NotYetImplemented();
        public static bool operator >(IntSet a, IntSet b) => throw NotYetImplemented();

        public static IntSet operator +(IntSet a, IntSet b) => new(a, b);
        public static IntSet operator +(IntSet a, int b) => new(a, b);
        public static IntSet operator +(int a, IntSet b) => new(b, a);
        public static IntSet operator -(IntSet a, IntSet b) => throw NotYetImplemented();
        public static IntSet operator -(IntSet a, int b) => throw NotYetImplemented();
    }

    public partial class CharSet
    {
        public static bool operator ==(CharSet a, CharSet b) => throw NotYetImplemented();
        public static bool operator !=(CharSet a, CharSet b) => throw NotYetImplemented();
        public static bool operator <(CharSet a, CharSet b) => throw NotYetImplemented();
        public static bool operator >(CharSet a, CharSet b) => throw NotYetImplemented();

        public static CharSet operator +(CharSet a, CharSet b) => new(a, b);
        public static CharSet operator +(CharSet a, char b) => new(a, b);
        public static CharSet operator +(char a, CharSet b) => new(b, a);
        public static CharSet operator -(CharSet a, CharSet b) => throw NotYetImplemented();
        public static CharSet operator -(CharSet a, char b) => throw NotYetImplemented();
    }

    public partial class StringSet
    {
        public static bool operator ==(StringSet a, StringSet b) => throw NotYetImplemented();
        public static bool operator !=(StringSet a, StringSet b) => throw NotYetImplemented();
        public static bool operator <(StringSet a, StringSet b) => throw NotYetImplemented();
        public static bool operator >(StringSet a, StringSet b) => throw NotYetImplemented();

        public static StringSet operator +(StringSet a, StringSet b) => throw NotYetImplemented();
        public static StringSet operator +(StringSet a, string b) => throw NotYetImplemented();
        public static StringSet operator +(string a, StringSet b) => throw NotYetImplemented();
        public static StringSet operator -(StringSet a, StringSet b) => throw NotYetImplemented();
        public static StringSet operator -(StringSet a, string b) => throw NotYetImplemented();
    }
}
