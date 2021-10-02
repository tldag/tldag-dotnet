using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class Alphabet : IReadOnlyList<int>
    {
        private readonly int[] map = new int[65536];
        private int[]? classes = null;

        public Alphabet() { }

        public Alphabet(IEnumerable<char> symbols)
        {
            FillMap(symbols);
        }

        private int[] GetClasses()
            => classes ??= map.Distinct().OrderBy(i => i).ToArray();

        public int this[int index] => map[index];

        public int Count => GetClasses().Length;

        private void FillMap(IEnumerable<char> symbols)
        {
            char[] chars = symbols.Distinct().OrderBy(c => c).ToArray();

            for (int i = 0, n = chars.Length, j = 1; i < n; ++i, ++j)
            {
                map[chars[i]] = j;
            }
        }

        public IEnumerator<int> GetEnumerator() => GetClassesEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetClassesEnumerator();
        private IEnumerator<int> GetClassesEnumerator() => GetClasses().AsEnumerable().GetEnumerator();

        public void Save(Stream stream)
        {
            IntStream intStream = new(stream);

            intStream.Write(map);
        }

        public void Load(Stream stream)
        {
            IntStream intStream = new(stream);

            intStream.Read(map);
            classes = null;
        }
    }
}
