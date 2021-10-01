using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.Libraries.Core.Collections
{
    [TestClass]
    public class IntSetTests
    {
        [TestMethod]
        public void EmptySet()
        {
            IntSet set = new();

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        public void SingleElementSet()
        {
            IntSet set = new(2);

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(2, set[0]);
            Assert.IsTrue(set.Contains(2));
            Assert.IsFalse(set.Contains(3));
        }

        [TestMethod]
        public void SingleElementSetEnumerators()
        {
            IntSet set = new(2);

            IEnumerator<int> enumerator1 = set.GetEnumerator();

            Assert.IsTrue(enumerator1.MoveNext());
            Assert.AreEqual(2, enumerator1.Current);
            Assert.IsFalse(enumerator1.MoveNext());

            IEnumerator enumerator2 = ((IEnumerable)set).GetEnumerator();

            Assert.IsTrue(enumerator2.MoveNext());
            Assert.AreEqual(2, enumerator2.Current);
            Assert.IsFalse(enumerator2.MoveNext());
        }

        [TestMethod]
        public void Uniqueness()
        {
            IntSet set = new(2, 2, 2, 2);

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(2, set[0]);
            Assert.IsTrue(set.Contains(2));
        }

        [TestMethod]
        public void Uniqueness2()
        {
            IntSet set = new(2, 3, 4, 2, 3, 2, 3);

            Assert.AreEqual(3, set.Count);

            Assert.AreEqual(2, set[0]);
            Assert.AreEqual(3, set[1]);
            Assert.AreEqual(4, set[2]);

            Assert.IsTrue(set.Contains(2));
            Assert.IsTrue(set.Contains(3));
            Assert.IsTrue(set.Contains(4));
        }

        [TestMethod]
        public void FromList()
        {
            List<int> list = new() { 5, 4, 1, 5 };
            IntSet set = new(list);

            Assert.AreEqual(3, set.Count);

            Assert.IsFalse(set.Contains(2));
            Assert.IsFalse(set.Contains(0));
            Assert.IsFalse(set.Contains(7));
        }
    }
}
