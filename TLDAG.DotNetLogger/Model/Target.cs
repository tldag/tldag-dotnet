using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Target
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("tasks")]
        public BuildTasks? Tasks { get; set; } = null;

        public Target(int id) { Id = id; }
        public Target() : this(-1) { }
    }
}
