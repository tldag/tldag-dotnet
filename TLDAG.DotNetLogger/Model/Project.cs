using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Adapter;
using static TLDAG.DotNetLogger.Model.Support.ItemsSupport;
using static TLDAG.DotNetLogger.Model.Support.MessagesSupport;
using static TLDAG.DotNetLogger.Model.Support.PropertiesSupport;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Project : IHasMessages, IComparable<Project>
    {
        [XmlElement("file")]
        public string File { get; set; }

        [XmlAttribute("name")]
        public string Name
        {
            get => string.IsNullOrWhiteSpace(File) ? "" : Path.GetFileNameWithoutExtension(File);
            set { }
        }

        [XmlElement("messages")]
        public Messages? Messages { get; set; } = null;

        [XmlElement("globals")]
        public Properties? Globals { get; set; } = null;

        [XmlElement("properties")]
        public Properties? Properties { get; set; } = null;

        [XmlElement("items")]
        public Items? Items { get; set; } = null;

        [XmlElement("passes")]
        public Passes? Passes { get; set; } = null;

        public Project(string file) { File = file; }
        public Project() : this(string.Empty) { }

        public void AddMessage(string? message) { Messages = AddToMessages(Messages, message); }

        public int CompareTo(Project other) => StringComparer.Ordinal.Compare(Name, other.Name);
    }
}
