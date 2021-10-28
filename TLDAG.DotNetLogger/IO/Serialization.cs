using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Model;
using static System.Text.Encoding;

namespace TLDAG.DotNetLogger.IO
{
    public static class DnlSerialization
    {
        private static readonly XmlWriterSettings DefaultXmlWriterSettings
            = new() { Indent = false, Encoding = UTF8 };

        private static readonly XmlSerializer xmlSerializer = new(typeof(Log));

        public static string ToXml(Log log, XmlWriterSettings? settings = null)
        {
            settings ??= DefaultXmlWriterSettings;

            using MemoryStream stream = new();
            using XmlWriter xmlWriter = XmlWriter.Create(stream, settings);

            xmlSerializer.Serialize(xmlWriter, log, log.Namespaces);

            return settings.Encoding.GetString(stream.ToArray());
        }

        public static Log FromXml(string xml)
        {
            using StringReader reader = new(xml);

            return (Log)xmlSerializer.Deserialize(reader);
        }

        private static readonly BinaryFormatter binaryFormatter = new();

        public static byte[] ToBytes(Log log)
        {
            using MemoryStream stream = new();

            binaryFormatter.Serialize(stream, log);

            return stream.ToArray();
        }

        public static Log FromBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);

            return (Log)binaryFormatter.Deserialize(stream);
        }

        public static void Send(Log log, string pipeHandle)
        {
            byte[] bytes = ToBytes(log);
            using AnonymousPipeClientStream pipe = new(PipeDirection.Out, pipeHandle);
            using BytesPipeSender sender = new(pipe);

            sender.Send(bytes);
        }
    }
}
