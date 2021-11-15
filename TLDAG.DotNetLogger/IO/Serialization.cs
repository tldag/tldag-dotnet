using System;
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

        private static readonly XmlSerializer xmlSerializer = new(typeof(DnlLog));

        public static string ToXml(DnlLog log, XmlWriterSettings? settings = null)
        {
            settings ??= DefaultXmlWriterSettings;

            using MemoryStream stream = new();
            using XmlWriter xmlWriter = XmlWriter.Create(stream, settings);

            xmlSerializer.Serialize(xmlWriter, log, log.Namespaces);

            return settings.Encoding.GetString(stream.ToArray());
        }

        public static DnlLog FromXml(string xml)
        {
            using StringReader reader = new(xml);

            return xmlSerializer.Deserialize(reader) as DnlLog ?? throw new NotSupportedException();
        }

        private static readonly BinaryFormatter binaryFormatter = new();

        public static byte[] ToBytes(DnlLog log)
        {
            using MemoryStream stream = new();

#pragma warning disable SYSLIB0011 // Type or member is obsolete
            binaryFormatter.Serialize(stream, log);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            return stream.ToArray();
        }

        public static DnlLog FromBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
            return (DnlLog)binaryFormatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        }

        public static void Send(DnlLog log, string pipeHandle)
        {
            byte[] bytes = ToBytes(log);
            using AnonymousPipeClientStream pipe = new(PipeDirection.Out, pipeHandle);
            using BytesPipeSender sender = new(pipe);

            sender.Send(bytes);
        }
    }
}
