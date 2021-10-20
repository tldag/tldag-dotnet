using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using TLDAG.Build.DotNet;
using TLDAG.Build.Logging;
using TLDAG.Core;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.Tests.DotNet
{
    [TestClass]
    public class DotNetLoggingTests
    {
        [TestMethod]
        public void Test()
        {
            MSBuildEventReceiver receiver = new();
            FileInfo sln = Env.WorkingDirectory.GetFileAbove("tldag-dotnet-samples.sln");

            DotNetOptions options = new()
            {
                Loggers = { receiver.GetSenderInfo() }
            };

            ExecutionResult result = DotNetRunner.Restore(sln, options, true);

            Debug.WriteLine($"ExitCode: {result.ExitCode}");
            Debug.WriteLine($"Errors:{NewLine}{result.Errors.Join(NewLine)}");
            Debug.WriteLine($"Output:{NewLine}{result.Outputs.Join(NewLine)}");
        }
    }
}
