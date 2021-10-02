using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
