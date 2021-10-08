using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core.Tests
{
    [TestClass]
    public class ExceptionsTests
    {
        [TestMethod]
        public void TestCaller()
        {
            MethodBase expected = typeof(ExceptionsTests).GetMethod("TestCaller") ?? throw new NotSupportedException();
            MethodBase actual = GetCaller().GetMethod() ?? throw new NotSupportedException();

            Assert.AreEqual(expected, actual);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static StackFrame GetCaller() => new(1, true);
    }
}
