using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Model;
using static System.Text.Encoding;

namespace TLDAG.DotNetLogger.IO
{
    public static class Serialization
    {
        private static readonly XmlWriterSettings _xmlWriterSettings = new() { Indent = false, Encoding = UTF8 };
        private static readonly XmlSerializer _xmlSerializer = new(typeof(Build));

        public static string ToXml(Build build, XmlWriterSettings? settings = null)
        {
            settings ??= _xmlWriterSettings;

            using MemoryStream stream = new();
            using XmlWriter xmlWriter = XmlWriter.Create(stream, settings);

            _xmlSerializer.Serialize(xmlWriter, build, build.Namespaces);

            return settings.Encoding.GetString(stream.ToArray());
        }

        public static Build FromXml(string xml)
        {
            using StringReader reader = new(xml);

            return (Build)_xmlSerializer.Deserialize(reader);
        }

        private static readonly BinaryFormatter _binaryFormatter = new();

        public static byte[] ToBytes(Build build)
        {
            using MemoryStream stream = new();

            _binaryFormatter.Serialize(stream, build);

            return stream.ToArray();
        }

        public static Build FromBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);

            return (Build)_binaryFormatter.Deserialize(stream);
        }

        public static void Send(Build build, string pipeHandle)
        {
            byte[] bytes = ToBytes(build);
            using AnonymousPipeClientStream pipe = new(PipeDirection.Out, pipeHandle);
            BytesPipeSender sender = new(pipe);

            sender.Send(bytes);
        }
    }
}
