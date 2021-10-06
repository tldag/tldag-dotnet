using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.Code;
using static TLDAG.Libraries.Core.Code.Constants;

namespace TLDAG.Libraries.Core.Tests.Code
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void Empty()
        {
            Alphabet alphabet = new(Array.Empty<char>());

            Assert.AreEqual(2, alphabet.Count);
            Assert.AreEqual(Alphabet.EndOfFileClass, alphabet[EndOfFileChar]);
        }
    }
}
