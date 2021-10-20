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

            BuildResult? buildResult = receiver.GetResult();

            Assert.IsNotNull(buildResult);

            GetTestDirectory(true)
                .Combine("tldag-dotnet-samples.json")
                .WriteAllText(JsonConvert.SerializeObject(buildResult, Formatting.Indented));
        }
    }
}
