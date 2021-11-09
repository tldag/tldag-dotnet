using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Versioning;
using TLDAG.Build.DotNet;
using TLDAG.Core;

namespace TLDAG.Build.Tests.DotNet
{
    [TestClass]
    public class DotNetInfoTests
    {
        [TestMethod]
        public void Test()
        {
            DotNetInfo info = DotNetInfo.Get(Env.WorkingDirectory);
            SemanticVersion expectedVersion = new(6, 0, 100);
            SemanticVersion actualVersion = info.Version;

            Assert.AreEqual(expectedVersion, actualVersion);
            Assert.IsTrue(info.BaseDirectory.Exists);
        }
    }
}
