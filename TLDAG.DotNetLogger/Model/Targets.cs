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
        public List<Target> Entries { get; set; }

        internal Targets(List<Target>? entries) { Entries = entries ?? new(); }
        public Targets() : this(null) { }
    }
}
