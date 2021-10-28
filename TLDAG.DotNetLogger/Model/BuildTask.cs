using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class BuildTask
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("success")]
        public bool Success { get; set; }

        public BuildTask(int id) { Id = id; }
        public BuildTask() : this(-1) { }
    }
}
