using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Pass : IComparable<Pass>
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        public Pass(int id) { Id = id; }
        public Pass() : this(-1) { }

        public int CompareTo(Pass other) => Id.CompareTo(other.Id);
    }
}
