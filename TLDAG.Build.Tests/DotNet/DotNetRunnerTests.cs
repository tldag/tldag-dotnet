using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using TLDAG.Build.DotNet;
using TLDAG.Core;
using TLDAG.Core.Executing;
using TLDAG.Core.IO;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.Tests.DotNet
{
    [TestClass]
    public class DotNetRunnerTests
    {
        [TestMethod]
        public void Test()
        {
            FileInfo sln = Env.WorkingDirectory.GetFileAbove("tldag-dotnet-samples.sln");
            DotNetOptions options = new();

            ExecutionResult result = DotNetRunner.Restore(sln, options).ThrowOnError();

            Debug.WriteLine($"ExitCode: {result.ExitCode}");
            Debug.WriteLine($"Errors:{NewLine}{result.Errors.Join(NewLine)}");
            Debug.WriteLine($"Output:{NewLine}{result.Outputs.Join(NewLine)}");
        }
    }
}
