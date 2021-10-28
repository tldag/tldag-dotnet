using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Adapter;
using static TLDAG.DotNetLogger.Model.Support.MessagesSupport;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    [XmlRoot("log")]
    public class Log : IHasMessages
    {
        [XmlAttribute("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [XmlAttribute("transferred")]
        public int Transferred { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; } = false;

        [XmlElement("messages")]
        public Messages? Messages { get; set; } = null;

        [XmlElement("project")]
        public List<Project> Projects { get; set; } = new();

        public void AddMessage(string? message) { Messages = AddToMessages(Messages, message); }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get => namespaces; }

        private static readonly XmlSerializerNamespaces namespaces
            = new(new XmlQualifiedName[] { new("", "urn:log") });
    }
}
