using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TLDAG.Core.Executing;
using TLDAG.Core.Executing.Java;
using static TLDAG.Core.Executing.Java.JavaExecutable;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class JavaExecutablesTests
    {
        [TestMethod]
        public void TestTryGetVersion()
        {
            ExecutionResult result = ExecutionBuilder
                .Create(FindJava())
                .UseShellExecute(false).CreateNoWindow(true)
                .AddArgument("-version")
                .Build().Execute();

            Debug.WriteLine(result);
        }
    }
}
