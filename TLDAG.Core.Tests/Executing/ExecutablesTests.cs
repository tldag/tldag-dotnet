using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TLDAG.Core.Executing;

namespace TLDAG.Core.Tests.Executing
{
    [TestClass]
    public class ExecutablesTests
    {
        [TestMethod]
        public void FindDotnet()
        {
            Executable executable = Executables.Find("dotnet");

            Assert.IsTrue(File.Exists(executable.Path));
        }

        [TestMethod]
        public void FindDoesNotExist()
        {
            Assert.IsFalse(Executables.TryFind("_does_not_exist_", out Executable? executable));
            Assert.IsNull(executable);
        }
    }
}
