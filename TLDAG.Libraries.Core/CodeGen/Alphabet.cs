using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Libraries.Core.Collections;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class Alphabet : IReadOnlyList<int>
    {
        private readonly int[] map = new int[65536];

        private IntSet? classes = null;
        public IntSet Classes => classes ??= new(map);

        public Alphabet(IEnumerable<char> symbols)
        {
            FillMap(symbols);
        }

        internal Alphabet() { }

        public int this[int index] => map[index];

        public int Count => Classes.Count;

        private void FillMap(IEnumerable<char> symbols)
        {
            char[] chars = symbols.Distinct().OrderBy(c => c).ToArray();

            for (int i = 0, n = chars.Length, j = 1; i < n; ++i, ++j)
            {
                map[chars[i]] = j;
            }
        }

        public IEnumerator<int> GetEnumerator() => Classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Classes.GetEnumerator();

        public void Save(Stream stream)
        {
            IntStream output = new(stream);

            output.Write(map);
        }

        public static Alphabet Load(Stream stream)
        {
            IntStream input = new(stream);
            Alphabet alphabet = new();

            input.Read(alphabet.map);

            return alphabet;
        }
    }
}
