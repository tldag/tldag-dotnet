using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Internal;

namespace TLDAG.Core
{
    public static class Strings
    {
        public static string NewLine => Environment.NewLine;

        public static readonly StringComparison OrdinalComparison = StringComparison.Ordinal;
        public static readonly StringComparison OrdinalIgnoreCaseComparison = StringComparison.OrdinalIgnoreCase;

        public static readonly IComparer<string> OrdinalStringComparer = StringComparer.Ordinal;
        public static readonly IComparer<string> OrdinalIgnoreCaseComparer = StringComparer.OrdinalIgnoreCase;

        public static readonly Compare<string> CompareOrdinal
            = Delegates.ToCompare<string>(StringComparer.Ordinal);

        public static readonly Compare<string> CompareOrdinalIgnoreCase
            = Delegates.ToCompare<string>(StringComparer.OrdinalIgnoreCase);

        public static bool IsDigit(this char c) => c >= '0' && c <= '9';
        public static bool IsDigits(this string chars) => chars.All(c => c.IsDigit());

        public static IEnumerable<string> ToLines(this string text) => new StringToLines(text);

        public static string Join(this IEnumerable<string> strings, string separator)
            => string.Join(separator, strings);

        public static string Format(this string format, params object[] args) => string.Format(format, args);
    }
}
