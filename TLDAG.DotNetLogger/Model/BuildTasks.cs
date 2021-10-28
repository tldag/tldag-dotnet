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
        public List<BuildTask> Entries { get; set; }

        internal BuildTasks(List<BuildTask>? entries) { Entries = entries ?? new(); }
        public BuildTasks() : this(null) { }
    }
}
