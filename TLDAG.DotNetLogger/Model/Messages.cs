using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Messages
    {
        [XmlElement("message")]
        public List<string> Lines { get; set; } = new();
    }
}
