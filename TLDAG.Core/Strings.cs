﻿using System;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core
{
    public static class Strings
    {
        public static readonly Compare<string> CompareOrdinal
            = Delegates.ToCompare<string>(StringComparer.Ordinal);

        public static readonly Compare<string> CompareOrdinalIgnoreCase
            = Delegates.ToCompare<string>(StringComparer.OrdinalIgnoreCase);

        public static bool IsDigit(this char c) => c >= '0' && c <= '9';
        public static bool IsDigits(this string chars) => throw NotYetImplemented();
    }
}
