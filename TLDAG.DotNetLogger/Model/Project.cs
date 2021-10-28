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

        [XmlElement("items")]
        public Items? Items { get; set; } = null;

        [XmlElement("pass")]
        public List<DnlPass> Passes { get; set; } = new();

        public DnlProject(string file) { File = file; }
        public DnlProject() : this(string.Empty) { }

        public int CompareTo(DnlProject other) => StringComparer.Ordinal.Compare(Name, other.Name);
    }
}
