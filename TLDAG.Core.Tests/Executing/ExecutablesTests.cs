using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Core.Executing;
using static TLDAG.Core.Executing.Executables;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class ExecutablesTests
    {
        [TestMethod]
        public void FindDotnet()
        {
            Executable executable = FindExecutable("dotnet");

            Assert.IsTrue(File.Exists(executable.Path));
        }

        [TestMethod]
        public void FindDoesNotExist()
        {
            Assert.IsFalse(TryFindExecutable("_does_not_exist_", out Executable? executable));
            Assert.IsNull(executable);
        }
    }
}
