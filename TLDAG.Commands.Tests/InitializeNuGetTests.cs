using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Test;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class InitializeNuGetTests : CommandTests
    {
        protected override Type CommandType => typeof(InitializeNuGet);

        [TestMethod]
        public void Test()
        {
            DirectoryInfo directory = GetTestDirectory();

            Invoke($"Initialize-NuGet {directory.FullName} -Defaults");
        }
    }
}
