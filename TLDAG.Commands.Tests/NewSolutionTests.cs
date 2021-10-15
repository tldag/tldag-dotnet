using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Test;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class NewSolutionTests : CommandTests
    {
        protected override Type CommandType => typeof(NewSolution);

        [TestMethod]
        public void TestNewSolution()
        {
            FileInfo expected = new("NewSolutionTests.sln");

            if (expected.Exists) expected.Delete();

            FileInfo actual = Invoke<FileInfo>($"New-Solution '{expected.FullName}'");

            Assert.IsTrue(actual.Exists);
            Assert.AreEqual(expected.FullName, actual.FullName);
        }
    }
}
