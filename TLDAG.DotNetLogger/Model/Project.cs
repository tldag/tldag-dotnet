using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Model.Support.ItemsSupport;
using static TLDAG.DotNetLogger.Model.Support.PropertiesSupport;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Project : IHasGlobals, IHasProperties, IHasItems, IComparable<Project>
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

        [XmlElement("passes")]
        public Passes Passes { get; set; } = new();

        public Project(string file) { File = file; }
        public Project() : this("") { }

        public int CompareTo(Project other) => StringComparer.Ordinal.Compare(Name, other.Name);

        public void SetGlobals(IEnumerable<StringEntry>? source)
            { Globals = CreateProperties(source, FilterProperty); }

        public void SetProperties(IEnumerable<StringEntry>? source)
            { Properties = CreateProperties(source, FilterProperty); }

        public void SetItems(IEnumerable<Item>? source)
            { Items = CreateItems(source); }
    }
}
