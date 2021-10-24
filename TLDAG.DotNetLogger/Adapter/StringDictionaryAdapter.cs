using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class StringDictionaryAdapter
    {
        private IDictionary<string, string>? dictionary;
        public IDictionary<string, string> Dictionary => dictionary ??= new Dictionary<string, string>();

        public StringDictionaryAdapter(IDictionary<string, string>? dictionary) { this.dictionary = dictionary; }
    }
}
