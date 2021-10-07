using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TLDAG.Core.Code;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Tests.Code
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
