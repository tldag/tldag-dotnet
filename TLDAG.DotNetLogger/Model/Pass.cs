using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Pass : IComparable<Pass>
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("globals")]
        public Properties Globals { get; set; } = new();

        [XmlElement("properties")]
        public Properties Properties { get; set; } = new();

        [XmlElement("targets")]
        public Targets Targets { get; set; } = new();

        [XmlElement("items")]
        public Items Items { get; set; } = new();

        public Pass(int id) { Id = id; }
        public Pass() : this(-1) { }

        public int CompareTo(Pass other) => Id.CompareTo(other.Id);
    }
}
