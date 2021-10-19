using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core.Exceptions;

namespace TLDAG.Core.Tests.Exceptions
{
    [TestClass]
    public class ExecutionExceptionTests
    {
        [TestMethod]
        public void Test()
        {
            string[] lines = { "foo", "bar" };
            ExecutionException exception = new(1, lines);
            string actual = exception.Message;

            Assert.IsTrue(actual.Contains("1"));
            Assert.IsTrue(actual.Contains("foo"));
            Assert.IsTrue(actual.Contains("bar"));

            Debug.WriteLine(actual);
        }
    }
}
