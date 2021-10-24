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

        [XmlElement("target")]
        public List<Target> Targets { get; set; } = new();

        public Pass(int id) { Id = id; }
        public Pass() : this(-1) { }

        public Target AddTarget(string? name, int id)
        {
            if (name is null || string.IsNullOrWhiteSpace(name))
                return new();

            Target target = new(name, id);
            
            Targets.Add(target);
            
            return target;
        }

        public Target? GetTarget(int id) => Targets.Where(t => t.Id == id).LastOrDefault();

        public int CompareTo(Pass other) => Id.CompareTo(other.Id);
    }
}
