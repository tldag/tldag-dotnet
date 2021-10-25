using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Item : IComparable<Item>
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("spec")]
        public string Spec { get; set; }

        [NonSerialized]
        private string? key = null;

        [XmlIgnore]
        public string Key { get => key ??= CreateKey(Type, Spec); }

        public Item(string type, string spec) { Type = type; Spec = spec; }
        public Item() : this(string.Empty, string.Empty) { }

        public int CompareTo(Item other) => Key.CompareTo(other.Key);

        public static string CreateKey(string type, string spec) => $"{type}.{spec}";
    }
}
