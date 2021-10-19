using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TLDAG.Core.Reflection;

namespace TLDAG.Core.Tests.Reflection
{
    [TestClass]
    public class ExecutablesTests
    {
        [TestMethod]
        public void TestLookupPath()
        {
            Assert.IsTrue(Executables.LookupPath.Count() > 1);
        }

        [TestMethod]
        public void FindDotnet()
        {
            Executable executable = Executables.Find("dotnet");

            Assert.IsTrue(File.Exists(executable.Path));
        }
    }
}
