using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.Code;
using static TLDAG.Libraries.Core.Code.CodeConstants;

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
            Assert.AreEqual(EndOfFileName, enumerator.Current.Name);
        }
    }
}
