using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core.Cryptography;

namespace TLDAG.Core.Tests.Cryptography
{
    [TestClass]
    public class RandomsTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            double sum = 0.0;
            int n = 1000;

            for (int i = 0; i < n; ++i) sum += Randoms.NextDouble();

            Debug.WriteLine(sum / n);
        }
    }
}
