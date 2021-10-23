using System;
using System.Xml.Serialization;
using static System.StringComparer;

namespace TLDAG.DotNetLogger.Model
{
    public class Item : IComparable<Item>, IEquatable<Item>
    {
        [XmlAttribute("spec")]
        public string Spec { get; set; }

        public Item(ItemData data) { Spec = data.Spec; }
        public Item() { Spec = string.Empty; }

        public override int GetHashCode() => Ordinal.GetHashCode(Spec);
        public int CompareTo(Item other) => Ordinal.Compare(Spec, other.Spec);
        public override bool Equals(object obj) => EqualsTo(obj as Item);
        public bool Equals(Item other) => EqualsTo(other);
        private bool EqualsTo(Item? other) => other is not null && CompareTo(other) == 0;
    }
}
