using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TLDAG.Core.Globalization;

namespace TLDAG.Core.Tests.Globalization
{
    [TestClass]
    public class CurrencyTests
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsTrue(Currency.Currencies.Count() > 0);
        }
    }
}
