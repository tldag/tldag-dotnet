using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    [TestClass]
    public class SourceLinesTests
    {
        [TestMethod]
        public void Empty()
        {
            SourceLinesOld lines = new("");
            char[] expected = Array.Empty<char>();
            SourceCharacterOld[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void OneLine()
        {
            SourceLinesOld lines = new("1");
            char[] expected = { '1' };
            SourceCharacterOld[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultiLine()
        {
            SourceLinesOld lines = new("1\r2\r\n3\n4");
            char[] expected = { '1', '\n', '2', '\n', '3', '\n', '4' };
            SourceCharacterOld[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        private static void AreEqual(char[] expected, SourceCharacterOld[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0, n = expected.Length; i < n; ++i)
            {
                Assert.AreEqual(expected[i], actual[i].Value);
            }
        }
    }
}
