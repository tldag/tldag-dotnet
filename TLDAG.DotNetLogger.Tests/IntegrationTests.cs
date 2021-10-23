using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using TLDAG.DotNetLogger.Model;
using TLDAG.Test;
using static TLDAG.DotNetLogger.IO.Serialization;

namespace TLDAG.DotNetLogger.Tests
{
    [TestClass]
    public class IntegrationTests : TestsBase
    {
        private static readonly XmlWriterSettings writerSettings
            = new() { Indent = true, IndentChars = "  ", Encoding = Encoding.UTF8 };

        [TestMethod]
        public void Test()
        {
            FileInfo solutionFile = SolutionDirectory.Combine("tldag-dotnet-samples.sln");
            DirectoryInfo directory = GetTestDirectory(true);

            List<Build> builds = new();

            using Receiver receiver = new((_, e) => { builds.Add(e.Build); });

            ExecutionBuilder.Create("dotnet")
                .UseShellExecute(false)
                .CreateNoWindow(true)
                .WorkingDirectory(SolutionDirectory)
                .AddArgument("build")
                .AddArgument($"\"{solutionFile.FullName}\"")
                .AddArgument($"-logger:{receiver.SenderDescriptor}")
                .Build()
                .Execute(true);

            while (builds.Count < 1) Task.Delay(10).Wait();

            for (int i = 0; i < builds.Count; ++i)
            {
                FileInfo file = directory.Combine($"{i}.xml");

                file.WriteAllText(ToXml(builds[i], writerSettings));
            }
        }
    }
}
