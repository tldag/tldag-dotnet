using System;
using System.Collections;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public partial class Alphabet : IEnumerable<int>
    {
        public readonly CharSet Symbols;

        private readonly int[] map = new int[65536];
        private readonly IntSet classes;

        public readonly int Count;

        public Alphabet(IEnumerable<char> symbols)
        {
            Symbols = new(symbols);

            int nextId = 0;

            foreach (char c in Symbols) map[c] = ++nextId;
            classes = new(map);
            Count = classes.Count;
        }

        public int this[char key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<int> GetEnumerator() => classes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}