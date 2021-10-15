﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core.Tests
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void TestToDenseString()
        {
            DateTime dateTime = new(2021, 10, 15, 16, 32, 12, 55);
            string expected = "20211015163212055";
            string actual = dateTime.ToDenseString();

            Assert.AreEqual(expected, actual);
        }
    }
}
