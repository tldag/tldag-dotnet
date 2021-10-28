using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlTask : DnlElement
    {
        [XmlAttribute("ident")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("success")]
        public bool Success { get; set; }

        public DnlTask(int id) { Id = id; }
        public DnlTask() : this(-1) { }
    }

    [Serializable]
    public class DnlTasks
    {
        [XmlElement("task")]
        public List<DnlTask> Entries { get; set; }

        internal DnlTasks(List<DnlTask>? entries) { Entries = entries ?? new(); }
        public DnlTasks() : this(null) { }
    }
}
