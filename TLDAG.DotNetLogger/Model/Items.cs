using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Algorithm.Algorithms;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Items
    {
        [XmlElement("item")]
        public List<Item> Entries { get; set; } = new();

        public void AddOrReplace(Item item)
        {
            Remove(item.Key);
            Entries.Add(item);
            Entries.Sort();
        }

        public void AddOrReplace(IEnumerable<Item> items)
        {
            foreach (Item item in items)
            {
                Remove(item.Key);
                Entries.Add(item);
            }

            Entries.Sort();
        }

        public void Remove(string key) { RemoveWhere(Entries, item => item.Key.Equals(key)); }
    }
}
