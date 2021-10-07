using System;
using System.Collections;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public partial class Alphabet : IEnumerable<int>
    {
        public const int OtherClass = 0;
        public const int EndOfFileClass = 1;

        public readonly CharSet Symbols;

        private readonly int[] map;
        private readonly IntSet classes;

        public readonly int Count;

        public Alphabet(IEnumerable<char> symbols)
        {
            Symbols = new(symbols);

            int nextId = 2;

            map = new int[65536];
            foreach (char c in Symbols) map[c] = nextId++;
            map[EndOfFileChar] = EndOfFileClass;

            classes = new(map);
            Count = classes.Count;
        }

        public int this[char key] => map[key];

        public IEnumerator<int> GetEnumerator() => classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => classes.GetEnumerator();
    }
}