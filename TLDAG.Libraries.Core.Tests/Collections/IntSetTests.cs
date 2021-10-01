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

        [TestMethod]
        public void ContainsAny()
        {
            IntSet set1 = new(5, 4, 3);
            IntSet set2 = new(3, 2, 1);

            Assert.IsTrue(set1.ContainsAny(set2));
        }

        [TestMethod]
        public void Add()
        {
            IntSet set = new();

            set += 2;
            Assert.AreEqual(1, set.Count);

            set += 2;
            Assert.AreEqual(1, set.Count);

            set += 4;
            Assert.AreEqual(2, set.Count);

            set += 3;
            Assert.AreEqual(3, set.Count);

            set += 1;
            Assert.AreEqual(4, set.Count);
        }

        [TestMethod]
        public void Add2()
        {
            IntSet set1 = new(5, 4, 3);
            IntSet set2 = new(3, 2, 1);
            IntSet set = set1 + set2;

            Assert.AreEqual(5, set.Count);
            Assert.IsTrue(set.ContainsAll(set1));
            Assert.IsTrue(set.ContainsAll(set2));
        }

        [TestMethod]
        public void Sub()
        {
            IntSet set = new(5, 4, 3);
            IntSet expected, actual;

            expected = new(5, 4, 3);
            actual = set - 0;
            Assert.AreEqual(expected, actual);

            expected = new(5, 4, 3);
            actual = set - 7;
            Assert.AreEqual(expected, actual);

            expected = new(5, 4);
            actual = set - 3;
            Assert.AreEqual(expected, actual);

            expected = new(5, 3);
            actual = set - 4;
            Assert.AreEqual(expected, actual);

            expected = new(4, 3);
            actual = set - 5;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Sub2()
        {
            IntSet set = new(5, 4, 3);
            IntSet expected, actual;

            expected = set;
            actual = set - new IntSet();
            Assert.AreEqual(expected, actual);

            expected = set;
            actual = set - new IntSet(2);
            Assert.AreEqual(expected, actual);

            expected = set;
            actual = set - new IntSet(6);
            Assert.AreEqual(expected, actual);

            expected = new(5);
            actual = set - new IntSet(3, 4);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Intersect()
        {
            IntSet set1 = new();
            IntSet set2 = new(5, 4, 3);
            IntSet set3 = new(3, 4, 7);
            IntSet expected, actual;

            expected = new();
            actual = set1 * set2;
            Assert.AreEqual(expected, actual);

            expected = set2;
            actual = set2 * set2;
            Assert.AreEqual(expected, actual);

            expected = new(3, 4);
            actual = set2 * set3;
            Assert.AreEqual(expected, actual);
        }
    }
}
