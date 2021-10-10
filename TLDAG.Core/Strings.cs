using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core
{
    public static class Strings
    {
        public static readonly Compare<string> CompareOrdinal
            = Delegates.ToCompare<string>(StringComparer.Ordinal);

        public static readonly Compare<string> CompareOrdinalIgnoreCase
            = Delegates.ToCompare<string>(StringComparer.OrdinalIgnoreCase);
    }
}
