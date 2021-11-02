using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Core.Executing;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class ExecutablesTests
    {
        [TestMethod]
        public void FindDotnet()
        {
            Executable executable = Executable.Find("dotnet");

            Assert.IsTrue(File.Exists(executable.Path));
        }

        [TestMethod]
        public void FindDoesNotExist()
        {
            Assert.IsFalse(Executable.TryFind("_does_not_exist_", out Executable? executable));
            Assert.IsNull(executable);
        }
    }
}
