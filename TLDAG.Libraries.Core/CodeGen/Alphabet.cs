using System;
using System.Collections;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
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