using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Infos
    {
        [XmlElement("info")]
        public List<string> Entries { get; set; } = new();

        public void Add(string info) { Entries.Add(info); }
    }
}
