using System.IO;
using System.IO.Pipes;
using System.Xml;
using System.Xml.Serialization;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.IO
{
    public class ResultSerializer
    {
        public static XmlSerializer Serializer { get; } = new(typeof(Result));
        public static XmlWriterSettings Settings { get => new() { Indent = false }; }

        public static string Serialize(Result result, XmlWriterSettings? settings = null)
        {
            settings ??= Settings;

            using StringWriter stringWriter = new();
            using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

            Serializer.Serialize(xmlWriter, result, result.Namespaces);

            return stringWriter.ToString();
        }

        public static Result Deserialize(string xml)
        {
            using StringReader reader = new(xml);

            return (Result)Serializer.Deserialize(reader);
        }

        public static void Send(Result result, string pipeHandle)
        {
            using AnonymousPipeClientStream pipe = new(PipeDirection.Out, pipeHandle);
            StringPipeStream stream = new(pipe);

            stream.Write(Serialize(result));
        }
    }
}
