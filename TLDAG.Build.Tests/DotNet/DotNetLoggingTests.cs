using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using TLDAG.Build.DotNet;
using TLDAG.Build.Logging;
using TLDAG.Core;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using static TLDAG.Core.Strings;
using static TLDAG.Build.Logging.MSBuildEventModel;
using Newtonsoft.Json;
using TLDAG.Test;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace TLDAG.Build.Tests.DotNet
{
    [TestClass]
    public class DotNetLoggingTests : TestsBase
    {
        [TestMethod]
        public void Test()
        {
            using MSBuildEventReceiver receiver = new();
            FileInfo sln = Env.WorkingDirectory.GetFileAbove("tldag-dotnet-samples.sln");

            DotNetOptions options = new()
            {
                Loggers = { receiver.GetSenderLogger() }
            };

            ExecutionResult executionResult = DotNetRunner.Build(sln, options, true);

            Debug.WriteLine($"ExitCode: {executionResult.ExitCode}");
            Debug.WriteLine($"Errors:{NewLine}{executionResult.Errors.Join(NewLine)}");
            Debug.WriteLine($"Output:{NewLine}{executionResult.Outputs.Join(NewLine)}");

            BuildResult? buildResult = receiver.Result;

            DirectoryInfo directory = GetTestDirectory(true);
            SerializeToXml(directory, buildResult);

            buildResult = DeserializeFromXml(directory);
        }

        private void SerializeToXml(DirectoryInfo directory, BuildResult result)
        {
            XmlWriterSettings settings = new()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "  "
            };

            XmlSerializer serializer = new(typeof(BuildResult));
            FileInfo file = directory.Combine("tldag-dotnet-samples.xml");
            using FileStream stream = new(file.FullName, FileMode.Create);
            using XmlWriter writer = XmlWriter.Create(stream, settings);

            serializer.Serialize(writer, result);
        }

        private BuildResult? DeserializeFromXml(DirectoryInfo directory)
        {
            XmlSerializer serializer = new(typeof(BuildResult));
            FileInfo file = directory.Combine("tldag-dotnet-samples.xml");
            using FileStream stream = new(file.FullName, FileMode.Open);

            return serializer.Deserialize(stream) as BuildResult;
        }
    }
}
