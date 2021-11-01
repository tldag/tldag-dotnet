using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.Build.Valuating
{
    public partial class NcssReport
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get => namespaces; }

        public void SaveToXml(FileInfo output)
        {
            using XmlWriter writer = XmlWriter.Create(output.FullName, writerSettings);

            Serializer.Serialize(writer, this, Namespaces);
        }

        public static readonly XmlSerializer Serializer = new(typeof(NcssReport));

        private readonly XmlSerializerNamespaces namespaces
            = new(new XmlQualifiedName[] { new("", "urn:report") });

        private static readonly XmlWriterSettings writerSettings = new()
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "  "
        };
    }
}
