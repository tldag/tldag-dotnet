using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Core.Cryptography;
using static TLDAG.Core.Algorithms.Comparing;

namespace TLDAG.Core.Tests.Cryptography
{
    [TestClass]
    public class TripleDESTests
    {
        [TestMethod]
        public void Test()
        {
            string password = "123";
            byte[] plain = { 1, 2, 3, 4 };
            byte[] encrypted = Cryptor.Encrypt(plain, password);
            byte[] decrypted = Cryptor.Decrypt(encrypted, password);

            Assert.AreEqual(0, Compare(plain, decrypted));
        }
    }
}
