using System;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    [XmlRoot("build")]
    public class Build
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get => namespaces; }

        [XmlAttribute("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        private static readonly XmlSerializerNamespaces namespaces
            = new(new XmlQualifiedName[] { new("", "urn:build") });
    }
}
