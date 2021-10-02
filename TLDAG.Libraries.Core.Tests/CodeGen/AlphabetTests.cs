using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void CharCount()
        {
            int min = char.MinValue;
            int max = char.MaxValue;
            int count = max - min + 1;

            Assert.AreEqual(65536, count);
        }
    }
}
