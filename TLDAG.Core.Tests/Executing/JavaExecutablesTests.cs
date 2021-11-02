using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TLDAG.Core.Executing;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class JavaExecutablesTests
    {
        [TestMethod]
        public void TestTryGetVersion()
        {
            ExecutionResult result = ExecutionBuilder
                .Create(Java.Find())
                .UseShellExecute(false).CreateNoWindow(true)
                .AddArgument("-version")
                .Build().Execute();

            Debug.WriteLine(result);
        }
    }
}
