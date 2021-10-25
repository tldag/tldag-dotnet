using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class BuildTasks
    {
        [XmlElement("task")]
        public List<BuildTask> Entries { get; set; } = new();

        public BuildTask? Get(int id) => Entries.Where(t => t.Id == id).FirstOrDefault();

        public BuildTask Add(string? name, int id)
        {
            if (name is null || string.IsNullOrWhiteSpace(name) || id < 0)
                return new();

            BuildTask task = new(name, id);

            Entries.Add(task);

            return task;
        }
    }
}
