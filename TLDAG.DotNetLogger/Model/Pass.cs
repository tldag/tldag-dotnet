using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlPass : DnlElement, IComparable<DnlPass>
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("globals")]
        public Properties? Globals { get; set; } = null;

        [XmlElement("properties")]
        public Properties? Properties { get; set; } = null;

        [XmlElement("items")]
        public DnlItems? Items { get; set; }

        [XmlElement("targets")]
        public DnlTargets? Targets { get; set; }

        public DnlPass(int id) { Id = id; }
        public DnlPass() : this(-1) { }

        public int CompareTo(DnlPass other) => Id.CompareTo(other.Id);
    }

    [Serializable]
    public class DnlPasses
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("pass")]
        public List<DnlPass> Entries { get; set; }

        public DnlPasses(List<DnlPass>? entries) { Entries = entries ?? new(); }
        public DnlPasses() : this(null) { }
    }
}
