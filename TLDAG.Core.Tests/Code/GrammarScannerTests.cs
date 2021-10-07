using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TLDAG.Core.Code;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Tests.Code
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
