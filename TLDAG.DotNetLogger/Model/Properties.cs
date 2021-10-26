using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Model.Support;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Properties
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("entry")]
        public List<StringEntry> Entries { get; set; } = new();

        public void Set(IEnumerable<StringEntry> source)
        {
            SortedSet<StringEntry> set = new();
            IEnumerable<StringEntry> filtered = source.Where(p => !RestrictedProperties.Contains(p.Key));

            foreach (StringEntry entry in filtered)
            {
                set.Remove(entry);
                set.Add(entry);
            }

            Entries = set.ToList();
        }
    }
}
