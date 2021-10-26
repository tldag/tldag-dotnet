using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Item : IEquatable<Item>, IComparable<Item>
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("spec")]
        public string Spec { get; set; }

        [NonSerialized]
        private string? key = null;

        [XmlIgnore]
        public string Key { get => key ??= $"{Type}.{Spec}"; }

        public Item(string type, string spec) { Type = type; Spec = spec; }
        public Item() : this(string.Empty, string.Empty) { }

        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object obj) => EqualsTo(obj as Item);
        public bool Equals(Item other) => EqualsTo(other);
        private bool EqualsTo(Item? other) => other is not null && Key.Equals(other.Key);
        public int CompareTo(Item other) => Key.CompareTo(other.Key);

        public static bool operator ==(Item a, Item b) => a.EqualsTo(b);
        public static bool operator !=(Item a, Item b) => !a.EqualsTo(b);

        public static bool operator <(Item a, Item b) => a.CompareTo(b) < 0;
        public static bool operator <=(Item a, Item b) => a.CompareTo(b) <= 0;
        public static bool operator >(Item a, Item b) => a.CompareTo(b) > 0;
        public static bool operator >=(Item a, Item b) => a.CompareTo(b) >= 0;
    }
}
