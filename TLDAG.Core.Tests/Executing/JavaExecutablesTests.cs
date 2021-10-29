using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TLDAG.Core.Executing;
using TLDAG.Core.Executing.Java;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class JavaExecutablesTests
    {
        [TestMethod]
        public void TestTryFind()
        {
            Assert.IsTrue(JavaExecutables.TryFind(out Executable _));
        }

        [TestMethod]
        public void TestFind()
        {
            JavaExecutables.Find();
        }

        [TestMethod]
        public void TestTryGetVersion()
        {
            ExecutionResult result = ExecutionBuilder
                .Create(JavaExecutables.Find())
                .AddArgument("-version")
                .Build().Execute();

            Debug.WriteLine(result);
        }
    }
}
