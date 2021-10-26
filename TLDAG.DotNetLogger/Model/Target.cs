using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Target
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("tasks")]
        public BuildTasks? Tasks { get; set; } = null;

        public Target(string name, int id) { Name = name; Id = id; }
        public Target() : this(string.Empty, -1) { }

        public BuildTask? GetTask(int id) => Tasks?.Get(id);

        public BuildTask AddTask(string? name, int id)
        {
            if (name is null || string.IsNullOrWhiteSpace(name) || id < 0)
                return new();

            Tasks ??= new();

            return Tasks.Add(name, id);
        }
    }
}
