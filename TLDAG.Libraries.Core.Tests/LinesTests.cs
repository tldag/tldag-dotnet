using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.Libraries.Core.Tests
{
    [TestClass]
    public class LinesTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            string source = "1\r2\r\n3\n4";
            StringLines lines = new(source);

            Assert.AreEqual(4, lines.Count());

            IEnumerator<string> enumerator = lines.GetEnumerator();

            for (int i = 1; i <= 4; ++i)
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(i.ToString(), enumerator.Current);
            }

            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
