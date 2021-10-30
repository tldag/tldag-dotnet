using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TLDAG.Drawing.Coloring;
using TLDAG.Test;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class ConvertFromHslaToRgbaTests : CommandTests
    {
        protected override Type CommandType => typeof(ConvertFromHslaToRgba);

        [TestMethod]
        public void TestConvertFromHslaToRgba()
        {
            Rgba rgba = Invoke<Rgba>("ConvertFrom-HslaToRgba");

            Assert.AreEqual("rgba(0, 0, 0, 0)", rgba.ToString());
        }
    }
}
