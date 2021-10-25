using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Project : IComparable<Project>
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
        public Properties Globals { get; set; } = new();

        [XmlElement("properties")]
        public Properties Properties { get; set; } = new();

        [XmlElement("passes")]
        public Passes Passes { get; set; } = new();

        public Project(string file) { File = file; }
        public Project() : this("") { }

        public int CompareTo(Project other) => StringComparer.Ordinal.Compare(Name, other.Name);
    }
}
