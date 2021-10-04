using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Code
    {
        public static readonly StringSet ReservedTokenNames = new(new string[] { "", "EOF", "EOL" });
        public static readonly Regex TokenNameRegex = new("^[A-Z][_A-Z0-9]*$");

        public partial class Alphabet : IEnumerable<int>
        {
            public readonly CharSet Symbols;

            private readonly int[] map;
            private readonly IntSet classes;

            public Alphabet(IEnumerable<char> symbols)
            {
                Symbols = new(symbols);

                map = CreateMap(Symbols);
                classes = new(map);
            }

            public int this[char index]
            {
                get { throw new NotImplementedException(); }
            }

            public IEnumerator<int> GetEnumerator() => classes.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            private static int[] CreateMap(CharSet symbols)
            {
                throw new NotImplementedException();
            }
        }
    }
}
