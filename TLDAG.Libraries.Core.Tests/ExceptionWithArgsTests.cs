using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TLDAG.Libraries.Core.Tests
{
    [TestClass]
    public class ExceptionWithArgsTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            ExceptionWithArgs ex = new("Test {0}, {1}", "hello", 42);

            Assert.AreEqual("Test hello, 42", ex.Message);
        }
    }
}
