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
            SemanticVersion expectedVersion = new(6, 0, 100);
            SemanticVersion actualVersion = globalJson.Sdk.Version;
            GlobalJson.RollForwardValue expectedRollForward = GlobalJson.RollForwardValue.Minor;
            GlobalJson.RollForwardValue actualRollForward = globalJson.Sdk.RollForward;

            Assert.AreEqual(expectedVersion, actualVersion);
            Assert.AreEqual(expectedRollForward, actualRollForward);
        }
    }
}
