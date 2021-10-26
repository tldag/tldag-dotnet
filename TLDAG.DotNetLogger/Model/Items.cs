using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Items
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("item")]
        public List<Item> Entries { get; set; } = new();

        public void AddOrReplace(IEnumerable<Item> source)
        {
            SortedSet<Item> set = new(Entries);

            foreach (Item item in source)
                set.Remove(item);

            foreach (Item item in source)
                set.Add(item);

            Entries = set.ToList();
        }
    }
}
