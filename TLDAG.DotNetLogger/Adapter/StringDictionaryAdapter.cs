using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class StringDictionaryAdapter
    {
        public readonly IDictionary<string, string> Dictionary;

        public StringDictionaryAdapter(IDictionary<string, string>? dictionary)
        {
            Dictionary = dictionary ?? new Dictionary<string, string>();
        }
    }
}
