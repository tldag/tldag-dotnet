using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Core.Cryptography;
using TLDAG.Test;
using static TLDAG.Core.Algorithms.Comparing;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class ConvertToTripleDESEncryptedTests : CommandTests
    {
        protected override Type CommandType => typeof(ConvertToTripleDESEncrypted);

        private FileInfo PlainFile => new("ConvertToTripleDESEncryptedTests.txt");
        private FileInfo EncryptedFile => new("ConvertToTripleDESEncryptedTests.txt.enc");

        [TestMethod]
        public void TestEncrypt()
        {
            PlainFile.Delete();
            EncryptedFile.Delete();

            File.WriteAllText(PlainFile.FullName, "hello");

            byte[] plainBytes = File.ReadAllBytes(PlainFile.FullName);
            string password = "123";
            FileInfo result = Invoke<FileInfo>($"ConvertTo-TripleDESEncrypted '{PlainFile.FullName}' '{password}'");

            Assert.IsTrue(result.Exists);
            Assert.AreEqual(EncryptedFile.FullName, result.FullName);

            byte[] expected = TripleDES.Encrypt(plainBytes, password);
            byte[] actual = File.ReadAllBytes(EncryptedFile.FullName);

            Assert.AreEqual(0, Compare(expected, actual));
        }
    }
}
