using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            List<Log> logs = new();

            using Receiver receiver = new((_, e) => { logs.Add(e.Log); });

            ExecutionBuilder.Create("dotnet")
                .UseShellExecute(false)
                .CreateNoWindow(true)
                .WorkingDirectory(SolutionDirectory)
                .AddArgument("build")
                .AddArgument($"\"{solutionFile.FullName}\"")
                .AddArgument($"-logger:{receiver.SenderDescriptor}")
                .Build()
                .Execute(true);

            TimeSpan? timeout = Debugger.IsAttached ? null : TimeSpan.FromSeconds(4);

            receiver.Wait(1, timeout);

            for (int i = 0; i < logs.Count; ++i)
            {
                FileInfo file = directory.Combine($"{i}.xml");

                file.WriteAllText(ToXml(logs[i], writerSettings));
            }
        }
    }
}
