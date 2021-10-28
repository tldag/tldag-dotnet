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
using static TLDAG.DotNetLogger.IO.DnlSerialization;

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

            List<DnlLog> logs = new();

            using Receiver receiver = new((_, e) => { logs.Add(e.Log); });

            ExecutionResult result = ExecutionBuilder.Create("dotnet")
                .UseShellExecute(false)
                .CreateNoWindow(true)
                .WorkingDirectory(SolutionDirectory)
                .AddArgument("build")
                .AddArgument($"\"{solutionFile.FullName}\"")
                //.AddArgument("-target:Rebuild")
                .AddArgument($"-logger:{receiver.SenderDescriptor}")
                .SetEnvironmentVariable("DesignTimeBuild", "true")
                .SetEnvironmentVariable("BuildProjectReferences", "false")
                .SetEnvironmentVariable("SkipCompilerExecution", "true")
                .SetEnvironmentVariable("DisableRarCache", "true")
                .SetEnvironmentVariable("AutoGenerateBindingRedirects", "false")
                .SetEnvironmentVariable("CopyBuildOutputToOutputDirectory", "false")
                .SetEnvironmentVariable("CopyOutputSymbolsToOutputDirectory", "false")
                .SetEnvironmentVariable("SkipCopyBuildProduct", "false")
                .SetEnvironmentVariable("AddModules", "false")
                .SetEnvironmentVariable("UseCommonOutputDirectory", "true")
                .SetEnvironmentVariable("GeneratePackageOnBuild", "false")
                .Build()
                .Execute(false);

            TimeSpan? timeout = Debugger.IsAttached ? null : TimeSpan.FromSeconds(4);

            receiver.Wait(1, timeout);

            for (int i = 0; i < logs.Count; ++i)
            {
                FileInfo file = directory.Combine($"{i}.xml");

                file.WriteAllText(ToXml(logs[i], writerSettings));
            }

            Console.WriteLine(result.ElapsedTime);
            foreach (string line in result.Errors) Console.WriteLine(line);
            foreach (string line in result.Outputs) Console.WriteLine(line);

            result.ThrowOnError();
        }
    }
}
