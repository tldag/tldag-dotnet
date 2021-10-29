using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace TLDAG.Core.Tests
{
    [TestClass]
    public class EnvTests
    {
        [TestMethod]
        public void TestPath()
        {
            if (!Platform.IsWindows) return;

            DirectoryInfo dir = new(@"C:\Program Files\dotnet\");

            Assert.IsTrue(Env.GetPath().Select(d => d.FullName).Contains(dir.FullName));
        }
    }
}
