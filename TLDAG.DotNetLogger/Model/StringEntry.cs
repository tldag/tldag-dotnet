using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class StringEntry : IEquatable<StringEntry>, IComparable<StringEntry>
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public StringEntry(string key, string? value) { Key = key; Value = value ?? ""; }
        public StringEntry() : this(string.Empty, string.Empty) { }

        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object? obj) => EqualsTo(obj as StringEntry);
        public bool Equals(StringEntry? other) => EqualsTo(other);
        private bool EqualsTo(StringEntry? other) => other is not null && Key.Equals(other.Key);
        public int CompareTo(StringEntry? other) => Key.CompareTo(other?.Key);

        public static bool operator ==(StringEntry a, StringEntry b) => a.EqualsTo(b);
        public static bool operator !=(StringEntry a, StringEntry b) => !a.EqualsTo(b);

        public static bool operator <(StringEntry a, StringEntry b) => a.CompareTo(b) < 0;
        public static bool operator <=(StringEntry a, StringEntry b) => a.CompareTo(b) <= 0;
        public static bool operator >(StringEntry a, StringEntry b) => a.CompareTo(b) > 0;
        public static bool operator >=(StringEntry a, StringEntry b) => a.CompareTo(b) >= 0;
    }
}
