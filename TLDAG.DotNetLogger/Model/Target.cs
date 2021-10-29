using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlTarget : DnlElement
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("task")]
        public List<DnlTask> Tasks { get; set; } = new();

        public DnlTarget(int id) { Id = id; }
        public DnlTarget() : this(-1) { }
    }

    [Serializable]
    public class DnlTargets
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("target")]
        public List<DnlTarget> Entries { get; set; }

        public DnlTargets(List<DnlTarget>? entries) { Entries = entries ?? new(); }
        public DnlTargets() : this(null) { }
    }
}
