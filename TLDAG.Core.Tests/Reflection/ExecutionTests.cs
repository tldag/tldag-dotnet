using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Core.Reflection;

namespace TLDAG.Core.Tests.Reflection
{
    [TestClass]
    public class ExecutionTests
    {
        [TestMethod]
        public void Test()
        {
            Execution execution = ExecutionBuilder.Create("dotnet")
                .AddArgument("--info")
                .WorkingDirectory(Env.CurrentDirectory)
                .UseShellExecute(false)
                .Build();

            ExecutionResult result = execution.Execute();

            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Count > 0);
            Assert.AreEqual(0, result.Error.Count);
        }
    }
}
