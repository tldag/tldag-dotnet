using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Algorithm.Algorithms;
using static System.StringComparison;

namespace TLDAG.DotNetLogger.Model
{
    public class ItemList : IComparable<ItemList>, IEquatable<ItemList>
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("item")]
        public List<Item> Items { get; set; } = new();

        public ItemList(string type) { Type = type; }
        public ItemList() : this(string.Empty) { }

        public void AddOrReplace(ItemData data) { Remove(data.Spec); Add(data); }

        private void Remove(string spec) { RemoveWhere(Items, i => i.Spec.Equals(spec, Ordinal)); }
        private void Add(ItemData data) { Items.Add(new(data)); Items.Sort(); }

        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Type);
        public int CompareTo(ItemList other) => StringComparer.Ordinal.Compare(Type, other.Type);
        public override bool Equals(object obj) => EqualsTo(obj as ItemList);
        public bool Equals(ItemList other) => EqualsTo(other);
        private bool EqualsTo(ItemList? other) => other is not null && CompareTo(other) == 0;
    }
}
