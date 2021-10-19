using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Core.Resources;

namespace TLDAG.Core.Tests.Resources
{
    [TestClass]
    public class ExceptionResourcesTests
    {
        [TestMethod]
        public void Test()
        {
            string actual = ErrorsResources.InvalidStateFormat.Format("foo");

            Assert.IsTrue(actual.EndsWith("foo."));
        }
    }
}
