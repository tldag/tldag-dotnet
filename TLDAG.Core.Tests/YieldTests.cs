using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TLDAG.Core.Tests
{
    [TestClass]
    public class YieldTests
    {
        private class Disposable : IDisposable
        {
            private readonly Action disposeCallback;

            public Disposable(Action disposeCallback) { this.disposeCallback = disposeCallback; }
            ~Disposable() { Dispose(false); }
            public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
            private void Dispose(bool _) { disposeCallback(); }
        }

        private int disposed;
        private void OnDispose() { ++disposed; }

        private IEnumerable<int> Values
        {
            get
            {
                using Disposable disposable = new(OnDispose);

                yield return 1;
                yield return 2;
                yield return 3;
            }
        }

        [TestMethod]
        public void Test()
        {
            disposed = 0;

            foreach (int i in Values)
                Debug.WriteLine(i);

            Assert.AreEqual(1, disposed);
        }
    }
}
