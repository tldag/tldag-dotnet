using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlItem : IEquatable<DnlItem>, IComparable<DnlItem>
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("spec")]
        public string Spec { get; set; }

        [NonSerialized]
        private string? key = null;

        [XmlIgnore]
        public string Key { get => key ??= $"{Type}.{Spec}"; }

        public DnlItem(string type, string spec) { Type = type; Spec = spec; }
        public DnlItem() : this(string.Empty, string.Empty) { }

        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object obj) => EqualsTo(obj as DnlItem);
        public bool Equals(DnlItem other) => EqualsTo(other);
        private bool EqualsTo(DnlItem? other) => other is not null && Key.Equals(other.Key);
        public int CompareTo(DnlItem other) => Key.CompareTo(other.Key);

        public static bool operator ==(DnlItem a, DnlItem b) => a.EqualsTo(b);
        public static bool operator !=(DnlItem a, DnlItem b) => !a.EqualsTo(b);

        public static bool operator <(DnlItem a, DnlItem b) => a.CompareTo(b) < 0;
        public static bool operator <=(DnlItem a, DnlItem b) => a.CompareTo(b) <= 0;
        public static bool operator >(DnlItem a, DnlItem b) => a.CompareTo(b) > 0;
        public static bool operator >=(DnlItem a, DnlItem b) => a.CompareTo(b) >= 0;
    }

    [Serializable]
    public class DnlItems
    {
        [XmlAttribute("count")]
        public int Count { get => Entries.Count; set { } }

        [XmlElement("item")]
        public List<DnlItem> Entries { get; set; }

        public DnlItems(List<DnlItem>? entries) { Entries = entries ?? new(); }
        public DnlItems() : this(null) { }
    }
}
