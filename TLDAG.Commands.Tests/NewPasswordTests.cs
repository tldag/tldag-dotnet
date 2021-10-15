using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TLDAG.Test;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class NewPasswordTests : CommandTests
    {
        protected override Type CommandType => typeof(NewPassword);

        [TestMethod]
        public void TestNewPassword()
        {
            string password = Invoke<string>("New-Password");

            Assert.AreEqual(24, password.Length);
        }
    }
}
