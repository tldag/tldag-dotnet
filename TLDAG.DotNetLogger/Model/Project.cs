using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Project : IComparable<Project>
    {
        [XmlElement("file")]
        public string File { get; set; }

        [XmlAttribute("name")]
        public string Name
        {
            get => string.IsNullOrWhiteSpace(File) ? "" : Path.GetFileNameWithoutExtension(File);
            set { }
        }

        [XmlElement("pass")]
        public List<Pass> Passes { get; set; } = new();

        public Project(string file) { File = file; }
        public Project() : this("") { }

        public bool HasPass(int id) => Passes.Where(p => p.Id == id).Any();
        public Pass GetPass(int id) => Passes.Where(p => p.Id == id).First();

        public void AddPass(int id)
        {
            if (id >= 0)
            {
                Passes.Add(new(id));
                Passes.Sort();
            }
        }

        public int CompareTo(Project other) => StringComparer.Ordinal.Compare(Name, other.Name);
    }
}
