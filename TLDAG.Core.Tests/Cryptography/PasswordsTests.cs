using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Core.Cryptography;

namespace TLDAG.Core.Tests.Cryptography
{
    [TestClass]
    public class PasswordsTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            Assert.AreEqual(8, Passwords.NewPassword(0).Length);
        }
    }
}
