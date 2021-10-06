using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Code;
using static TLDAG.Libraries.Core.Code.Constants;

namespace TLDAG.Libraries.Core.Tests.Code
{
    [TestClass]
    public class GrammarScannerTests
    {
        [TestMethod]
        public void TestDevScanner()
        {
            Scanner scanner = Grammar.CreateDevScanner("");
            IEnumerator<Token> enumerator = scanner.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.Current.IsEndOfFile);
            Assert.AreEqual(EndOfFileName, enumerator.Current.Name);
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
