using System;
using System.Collections;
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

        [XmlElement("info")]
        public List<string>? Infos { get; set; } = null;

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

        public void SetGlobals(IEnumerable<StringEntry> source)
        {
            if (!source.Any()) return;

            Globals ??= new();
            Globals.Set(source);
        }

        public void SetProperties(IEnumerable<StringEntry> source)
        {
            if (!source.Any()) return;

            Properties ??= new();
            Properties.Set(source);
        }

        public void AddOrReplaceItems(IEnumerable<Item> source)
        {
            if (!source.Any()) return;

            Items ??= new();
            Items.AddOrReplace(source);
        }
    }
}
