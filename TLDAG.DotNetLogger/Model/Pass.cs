using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Adapter;
using static TLDAG.DotNetLogger.Model.Support.ItemsSupport;
using static TLDAG.DotNetLogger.Model.Support.MessagesSupport;
using static TLDAG.DotNetLogger.Model.Support.PropertiesSupport;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Pass : IHasMessages, IComparable<Pass>
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("messages")]
        public Messages? Messages { get; set; } = null;

        [XmlElement("globals")]
        public Properties? Globals { get; set; } = null;

        [XmlElement("properties")]
        public Properties? Properties { get; set; } = null;

        [XmlElement("items")]
        public Items? Items { get; set; } = null;

        [XmlElement("targets")]
        public Targets? Targets { get; set; } = null;

        public Pass(int id) { Id = id; }
        public Pass() : this(-1) { }

        public void AddMessage(string? message) { Messages = AddToMessages(Messages, message); }

        public int CompareTo(Pass other) => Id.CompareTo(other.Id);
    }
}
