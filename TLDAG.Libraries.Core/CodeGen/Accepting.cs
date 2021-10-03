using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class Accepting : IReadOnlyDictionary<int, string>
    {
        private readonly Dictionary<int, string> names;

        public IntSet Ids { get; }

        public IEnumerable<int> Keys => names.Keys;
        public IEnumerable<string> Values => names.Values;
        public int Count => names.Count;

        public string this[int id] => names[id];

        public Accepting(IReadOnlyDictionary<int, string> names)
        {
            this.names = new();

            foreach (int id in names.Keys)
            {
                this.names[id] = names[id];
            }

            Ids = new(this.names.Keys);
        }

        private Accepting(Dictionary<int, string> names, bool prepared)
        {
            if (!prepared) throw new InvalidOperationException();

            this.names = names;
            Ids = new(this.names.Keys);
        }

        public static Accepting Load(Stream stream)
        {
            Dictionary<int, string> names = new();
            using StreamReader reader = new(stream, Encoding.UTF8);
            string line = reader.ReadLine() ?? throw new InvalidOperationException();
            int count = int.Parse(line);

            for (int i = 0; i < count; ++i)
            {
                line = reader.ReadLine() ?? throw new InvalidOperationException();
                int colon = line.IndexOf(':');
                int id = int.Parse(line.Substring(0, colon));
                string name = line.Substring(colon + 1);

                names[id] = name;
            }

            return new(names, true);
        }

        public void Save(Stream stream)
        {
            StreamWriter writer = new(stream, Encoding.UTF8);

            writer.WriteLine(names.Count.ToString());

            foreach (KeyValuePair<int, string> kvp in names)
            {
                string line = kvp.Key + ":" + kvp.Value;

                writer.WriteLine(line);
            }
        }

        public bool ContainsKey(int key)
            => names.ContainsKey(key);

#if NET5_0_OR_GREATER
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out string value)
            => names.TryGetValue(key, out value);
#else
        public bool TryGetValue(int key, out string value)
            => names.TryGetValue(key, out value);
#endif

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
            => names.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => names.GetEnumerator();
    }
}
