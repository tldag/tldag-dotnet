using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    public class StringEntry
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public StringEntry(string key, string value) { Key = key; Value = value; }
        public StringEntry() : this("", "") { }
    }
}
