using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Test;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class NewSnkFileTests : CommandTests
    {
        protected override Type CommandType => typeof(NewSnkFile);

        private FileInfo SnkFile => new("NewSnkFileTests.snk");

        [TestMethod]
        public void TestNewSnkFile()
        {
            if (SnkFile.Exists) SnkFile.Delete();

            Invoke($"New-SnkFile '{SnkFile.FullName}'");

            Assert.IsTrue(SnkFile.Exists);
        }
    }
}
