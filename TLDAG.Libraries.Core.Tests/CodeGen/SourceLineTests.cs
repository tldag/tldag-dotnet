using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    [TestClass]
    public class SourceLineTests
    {
        [TestMethod]
        public void Empty()
        {
            SourceLine line = new(1, false, "");
            char[] expected = { '\n' };
            SourceCharacter[] actual = line.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyLast()
        {
            SourceLine line = new(1, true, "");
            char[] expected = Array.Empty<char>();
            SourceCharacter[] actual = line.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void Foo()
        {
            SourceLine line = new(1, false, "abc");
            char[] expected = { 'a', 'b', 'c', '\n' };
            SourceCharacter[] actual = line.ToArray();

            AreEqual(expected, actual);
        }

        [TestMethod]
        public void FooLast()
        {
            SourceLine line = new(1, true, "abc");
            char[] expected = { 'a', 'b', 'c' };
            SourceCharacter[] actual = line.ToArray();

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
