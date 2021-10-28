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
        public Items? Items { get; set; } = null;

        [XmlElement("target")]
        public List<DnlTarget> Targets { get; set; } = new();

        public DnlPass(int id) { Id = id; }
        public DnlPass() : this(-1) { }

        public int CompareTo(DnlPass other) => Id.CompareTo(other.Id);
    }
}
