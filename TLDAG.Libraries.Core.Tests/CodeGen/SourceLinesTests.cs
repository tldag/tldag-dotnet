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
            SourceLines lines = new("");
            char[] expected = Array.Empty<char>();
            SourceCharacter[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void OneLine()
        {
            SourceLines lines = new("1");
            char[] expected = { '1' };
            SourceCharacter[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultiLine()
        {
            SourceLines lines = new("1\r2\r\n3\n4");
            char[] expected = { '1', '\n', '2', '\n', '3', '\n', '4' };
            SourceCharacter[] actual = lines.ToArray();

            AreEqual(expected, actual);
        }

        private static void AreEqual(char[] expected, SourceCharacter[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0, n = expected.Length; i < n; ++i)
            {
                Assert.AreEqual(expected[i], actual[i].Value);
            }
        }
    }
}
