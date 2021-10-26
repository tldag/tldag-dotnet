using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Model.Support.SortedSetList;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Properties
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("entry")]
        public List<StringEntry> Entries { get; set; }

        public Properties() : this(new()) { }
        internal Properties(List<StringEntry> entries) { Entries = entries; }

        public void Set(IEnumerable<StringEntry>? source) { Entries = CreateSortedSetList(source); }
    }
}
