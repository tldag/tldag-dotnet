using System;
using System.Collections.Generic;

namespace TLDAG.Core.Algorithms
{
    public static class Algorithms
    {
        public static IComparer<string> OrdinalStringComparer => StringComparer.Ordinal;
        public static IComparer<string> OrdinalIgnoreCaseComparer => StringComparer.OrdinalIgnoreCase;



    }
}
