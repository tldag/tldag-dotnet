using System;
using System.Collections;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public partial class Alphabet : IEnumerable<uint>
    {
        public const uint OtherClass = 0;
        public const uint EndOfFileClass = 1;

        public readonly CharSet Symbols;

        private readonly uint[] map;
        private readonly UIntSet classes;

        public readonly int Count;

        public Alphabet(IEnumerable<char> symbols)
        {
            Symbols = new(symbols);

            uint nextId = 2;

            map = new uint[65536];
            foreach (char c in Symbols) map[c] = nextId++;
            map[EndOfFileChar] = EndOfFileClass;

            classes = new(map);
            Count = classes.Count;
        }

        public uint this[char key] => map[key];

        public IEnumerator<uint> GetEnumerator() => classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => classes.GetEnumerator();
    }
}