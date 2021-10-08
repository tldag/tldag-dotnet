using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Automation;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class NewSolutionTests : CommandTests
    {
        protected override Type CommandType => typeof(NewSolution);

        [TestMethod]
        public void MyTestMethod()
        {
            FileInfo expected = new("NewSolutionTests.sln");

            if (expected.Exists) expected.Delete();

            FileInfo actual = Invoke<FileInfo>($"New-Solution '{expected.FullName}'");

            Assert.IsTrue(actual.Exists);
            Assert.AreEqual(expected.FullName, actual.FullName);
        }
    }
}
