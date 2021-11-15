using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlProject : DnlElement, IComparable<DnlProject>
    {
        [XmlElement("file")]
        public string File { get; set; }

        [XmlAttribute("name")]
        public string Name
        {
            get => string.IsNullOrWhiteSpace(File) ? "" : Path.GetFileNameWithoutExtension(File);
            set { }
        }

        [XmlElement("globals")]
        public Properties? Globals { get; set; } = null;

        [XmlElement("properties")]
        public Properties? Properties { get; set; } = null;

        [XmlElement("passes")]
        public DnlPasses? Passes { get; set; }

        public DnlProject(string file) { File = file; }
        public DnlProject() : this(string.Empty) { }

        public int CompareTo(DnlProject? other) => StringComparer.Ordinal.Compare(Name, other?.Name);
    }

    [Serializable]
    public class DnlProjects
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("project")]
        public List<DnlProject> Entries { get; set; }

        public DnlProjects(List<DnlProject>? entries) { Entries = entries ?? new(); }
        public DnlProjects() : this(null) { }
    }
}
