using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    [XmlRoot("log")]
    public class DnlLog : DnlElement
    {
        [XmlAttribute("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [XmlAttribute("transferred")]
        public int Transferred { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; } = false;

        [XmlElement("projects")]
        public DnlProjects? Projects { get; set; }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get => namespaces; }

        private static readonly XmlSerializerNamespaces namespaces
            = new(new XmlQualifiedName[] { new("", "urn:log") });
    }
}
