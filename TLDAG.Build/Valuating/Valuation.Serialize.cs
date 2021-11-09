using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class Serializer
        {
            public Options Options { get; }

            public Serializer(Options options) { Options = options; }

            public virtual void Serialize(Report report, FileInfo file) { }
        }

        public class XSerializer : Serializer
        {
            public static XmlSerializer XmlSerializer { get; } = new(typeof(Report));

            public XSerializer(Options options) : base(options) { }

            public override void Serialize(Report report, FileInfo file)
            {
                using XmlWriter writer = XmlWriter.Create(file.FullName, GetSettings());

                XmlSerializer.Serialize(writer, this, report.Namespaces);
            }

            protected virtual XmlWriterSettings GetSettings()
                => new() { Encoding = Encoding.UTF8, Indent = true, IndentChars = "  " };
        }

        public class SerializerFactory : Factory
        {
            public SerializerFactory(Options options) : base(options) { }

            public virtual Serializer GetSerializer(FileInfo file)
            {
                return file.Extension switch
                {
                    ".xml" => new XSerializer(Options),
                    _ => new Serializer(Options)
                };
            }
        }
    }
}