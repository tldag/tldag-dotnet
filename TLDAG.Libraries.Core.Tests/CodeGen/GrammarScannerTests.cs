﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.Core.Tests.CodeGen
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
            Assert.AreEqual(Code.EOF, enumerator.Current.Name);
        }
    }
}
