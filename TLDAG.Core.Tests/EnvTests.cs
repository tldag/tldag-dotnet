using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core.Tests
{
    [TestClass]
    public class EnvTests
    {

        [TestMethod]
        public void TestPath()
        {
            if (!Env.IsWindows) return;

            DirectoryInfo dir = new(@"C:\Program Files\dotnet\");

            Assert.IsTrue(Env.Path.Select(d => d.FullName).Contains(dir.FullName));
        }
    }
}
