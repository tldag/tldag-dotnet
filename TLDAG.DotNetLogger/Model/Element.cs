using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlElement
    {
        [XmlElement("message")]
        public List<string> Messages { get; set; } = new();

        public void AddMessage(string? message)
        {
            if (message is null) return;
            if (string.IsNullOrWhiteSpace(message)) return;

            Messages.Add(message);
        }
    }
}
