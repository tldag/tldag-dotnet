using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.Algorithms
{
    public static class Algorithms
    {
        public static IComparer<string> DefaultStringComparer => StringComparer.Ordinal;
    }
}
