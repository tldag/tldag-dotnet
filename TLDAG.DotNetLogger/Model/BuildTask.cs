using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class BuildTask
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        public BuildTask(string name, int id) { Name = name; Id = id; }
        public BuildTask() : this(string.Empty, -1) { }
    }
}
