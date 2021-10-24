using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class StringEntry : IComparable<StringEntry>
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public StringEntry(string key, string value) { Key = key; Value = value; }
        public StringEntry() : this(string.Empty, string.Empty) { }

        public int CompareTo(StringEntry other) => Key.CompareTo(other.Key);
    }
}
