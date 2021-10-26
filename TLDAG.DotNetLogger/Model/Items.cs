using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Model.Support.SortedSetList;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Items
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("item")]
        public List<Item> Entries { get; set; }

        internal Items(List<Item> entries) { Entries = entries; }
        public Items() : this(new()) { }

        public void Set(IEnumerable<Item>? source) { Entries = CreateSortedSetList(source); }
    }
}
