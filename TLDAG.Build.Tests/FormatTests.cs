using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;

namespace TLDAG.Build.Tests
{
    [TestClass]
    public class FormatTests
    {
        [TestMethod]
        public void Test()
        {
            IFormatProvider formatProvider = new NumberFormatInfo()
            {
                CurrencyGroupSeparator = "'",
                CurrencySymbol = ""
            };

            string actual = string.Format(formatProvider, "{0:C0}", 55555);

            Debug.WriteLine(actual);
        }
    }
}
