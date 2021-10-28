using System;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class DnlElement
    {
        [XmlElement("messages")]
        public Messages? Messages { get; set; } = null;

        public void AddMessage(string? message)
        {
            if (message is null) return;
            if (string.IsNullOrWhiteSpace(message)) return;

            Messages ??= new();
            Messages.Lines.Add(message);
        }
    }
}
