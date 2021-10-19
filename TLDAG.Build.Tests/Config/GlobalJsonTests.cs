using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            GlobalJson.Get(Env.WorkingDirectory);
        }
    }
}
