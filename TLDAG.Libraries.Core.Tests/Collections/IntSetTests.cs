using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.Tests.Collections
{
    [TestClass]
    public class IntSetTests
    {
        [TestMethod]
        public void EmptySet()
        {
            IntSetOld set = IntSetOld.Empty;

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        public void SingleElementSet()
        {
            IntSetOld set = new(2);

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(2, set[0]);
            Assert.IsTrue(set.Contains(2));
            Assert.IsFalse(set.Contains(3));
        }

        [TestMethod]
        public void SingleElementSetEnumerators()
        {
            IntSetOld set = new(2);

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
            IntSetOld set = new(2, 2, 2, 2);

            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(2, set[0]);
            Assert.IsTrue(set.Contains(2));
        }

        [TestMethod]
        public void Uniqueness2()
        {
            IntSetOld set = new(2, 3, 4, 2, 3, 2, 3);

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
            IntSetOld set = new(list);

            Assert.AreEqual(3, set.Count);

            Assert.IsFalse(set.Contains(2));
            Assert.IsFalse(set.Contains(0));
            Assert.IsFalse(set.Contains(7));
        }

        [TestMethod]
        public void ContainsAny()
        {
            IntSetOld set1 = new(5, 4, 3);
            IntSetOld set2 = new(3, 2, 1);

            Assert.IsTrue(set1.ContainsAny(set2));
        }

        [TestMethod]
        public void Add()
        {
            IntSetOld set = IntSetOld.Empty;

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
            IntSetOld set1 = new(5, 4, 3);
            IntSetOld set2 = new(3, 2, 1);
            IntSetOld set = set1 + set2;

            Assert.AreEqual(5, set.Count);
            Assert.IsTrue(set.ContainsAll(set1));
            Assert.IsTrue(set.ContainsAll(set2));
        }

        [TestMethod]
        public void Sub()
        {
            IntSetOld set = new(5, 4, 3);
            IntSetOld expected, actual;

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
            IntSetOld set = new(5, 4, 3);
            IntSetOld expected, actual;

            expected = set;
            actual = set - IntSetOld.Empty;
            Assert.AreEqual(expected, actual);

            expected = set;
            actual = set - new IntSetOld(2);
            Assert.AreEqual(expected, actual);

            expected = set;
            actual = set - new IntSetOld(6);
            Assert.AreEqual(expected, actual);

            expected = new(5);
            actual = set - new IntSetOld(3, 4);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Intersect()
        {
            IntSetOld set1 = IntSetOld.Empty;
            IntSetOld set2 = new(5, 4, 3);
            IntSetOld set3 = new(3, 4, 7);
            IntSetOld expected, actual;

            expected = IntSetOld.Empty;
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
