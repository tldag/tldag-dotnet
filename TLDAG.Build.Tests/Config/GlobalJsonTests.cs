using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Versioning;
using TLDAG.Build.Config;
using TLDAG.Core;

namespace TLDAG.Build.Tests.Config
{
    [TestClass]
    public class GlobalJsonTests
    {
        [TestMethod]
        public void Test()
        {
            GlobalJson globalJson = GlobalJson.Get(Env.WorkingDirectory);
            SemanticVersion expected = new(5, 0, 402);
            SemanticVersion actual = globalJson.Sdk.Version;

            Assert.AreEqual(expected, actual);
        }
    }
}
