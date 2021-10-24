using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Algorithm.Algorithms;

namespace TLDAG.DotNetLogger.Model
{
    public class Properties
    {
        [XmlIgnore]
        public StringComparer Comparer { get; }

        [XmlElement("entry")]
        public List<StringEntry> Entries { get; set; } = new();

        public Properties(StringComparer comparer)
        {
            Comparer = comparer;
        }

        public Properties() : this(StringComparer.Ordinal) { }

        internal Properties(Properties source) : this(source, source.Comparer) { }

        internal Properties(Properties source, StringComparer comparer)
        {
            Comparer = comparer;
        }

        public void AddOrReplace(Dictionary<string, string> source)
        {
            Dictionary<string, string> target = GetDictionary();

            Merge(source, target);
            SetEntries(target);
        }

        private Dictionary<string, string> GetDictionary()
            => Entries.ToDictionary(e => e.Key, e => e.Value, Comparer);

        private void SetEntries(Dictionary<string, string> source)
            { Entries = source.Select(e => new StringEntry(e.Key, e.Value)).OrderBy(e => e.Key, Comparer).ToList(); }
    }
}
