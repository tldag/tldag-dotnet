using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Automation;
using TLDAG.Core.Algorithms;
using TLDAG.Core.IO;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class ConvertFromTripleDESEncryptedTests : CommandTests
    {
        protected override Type CommandType => typeof(ConvertFromTripleDESEncrypted);

        private FileInfo PlainFile => new("ConvertToTripleDESEncryptedTests.txt");
        private FileInfo EncryptedFile => new("ConvertToTripleDESEncryptedTests.txt.enc");

        [TestMethod]
        public void Test()
        {
            byte[] plainBytes = { 1, 2, 3, 4 };
            string password = "123";

            PlainFile.WriteAllBytes(plainBytes);
            Invoke($"ConvertTo-TripleDESEncrypted '{PlainFile.FullName}' '{password}'");
            PlainFile.Delete();

            FileInfo decodedFile = Invoke<FileInfo>($"ConvertFrom-TripleDESEncrypted '{EncryptedFile.FullName}' '{password}'");

            Assert.AreEqual(PlainFile.FullName, decodedFile.FullName);
            Assert.IsTrue(PlainFile.Exists);

            byte[] decoded = PlainFile.ReadAllBytes();

            Assert.IsTrue(Comparing.Compare(plainBytes, decoded) == 0);
        }
    }
}
