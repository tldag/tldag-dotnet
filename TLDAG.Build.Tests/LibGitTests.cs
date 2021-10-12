using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Build.Tests
{
    [TestClass]
    public class LibGitTests
    {
        [TestMethod]
        public void Is64BitProcess()
        {
            Debug.WriteLine($"Is64BitProcess: {Environment.Is64BitProcess}");
        }
    }
}
