using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TLDAG.Libraries.Core.Tests
{
    [TestClass]
    public class SemanticVersionTests
    {
        [TestMethod]
        public void Test1()
        {
            SemanticVersion version = SemanticVersion.Parse("1");

            AssertVersion(version, 1, 0, 0, "", "");
        }

        [TestMethod]
        public void Test12()
        {
            SemanticVersion version = SemanticVersion.Parse("1.2");

            AssertVersion(version, 1, 2, 0, "", "");
        }

        [TestMethod]
        public void Test123()
        {
            SemanticVersion version = SemanticVersion.Parse("1.2.3");

            AssertVersion(version, 1, 2, 3, "", "");
        }

        [TestMethod]
        public void Test1alpha()
        {
            SemanticVersion version = SemanticVersion.Parse("1-alpha");

            AssertVersion(version, 1, 0, 0, "alpha", "");
        }

        [TestMethod]
        public void Test1build()
        {
            SemanticVersion version = SemanticVersion.Parse("1+build");

            AssertVersion(version, 1, 0, 0, "", "build");
        }

        [TestMethod]
        public void Test1alphabuild()
        {
            SemanticVersion version = SemanticVersion.Parse("1-alpha+build");

            AssertVersion(version, 1, 0, 0, "alpha", "build");
        }

        private static void AssertVersion(SemanticVersion version, int major, int minor, int patch, string preRelease, string build)
        {
            Assert.AreEqual(major, version.Major);
            Assert.AreEqual(minor, version.Minor);
            Assert.AreEqual(patch, version.Patch);
            Assert.AreEqual(preRelease, version.PreRelease);
            Assert.AreEqual(build, version.Build);
        }

        [TestMethod]
        public void TestCompare()
        {
            SemanticVersion v1 = new("1.0");
            SemanticVersion v2 = new("1.0-alpha");

            Assert.IsTrue(v2 < v1);
        }

    }
}
