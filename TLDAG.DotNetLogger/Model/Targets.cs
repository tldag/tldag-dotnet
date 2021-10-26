using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Targets
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("target")]
        public List<Target> Entries { get; set; } = new();

        public Target? Get(int id) => Entries.Where(t => t.Id == id).LastOrDefault();

        public Target Add(string? name, int id)
        {
            if (name is null || string.IsNullOrWhiteSpace(name))
                return new();

            Target target = new(name, id);

            Entries.Add(target);

            return target;
        }
    }
}
