using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Passes
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("pass")]
        public List<Pass> Entries { get; set; }

        public Passes(List<Pass>? entries) { Entries = entries ?? new(); }
        public Passes() : this(null) { }
    }
}
