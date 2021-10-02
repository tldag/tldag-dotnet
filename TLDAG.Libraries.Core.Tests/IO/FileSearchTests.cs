using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.Tests.IO
{
    [TestClass]
    public class FileSearchTests
    {
        private int directoryStarted = 0;
        private int directoryEnded = 0;

        [TestMethod]
        public void Test()
        {
            DirectoryInfo directory = new(".");
            FileSearch search = new(directory, "*.dll", true);

            search.DirectoryStarted += Search_DirectoryStarted;
            search.DirectoryEnded += Search_DirectoryEnded;

            foreach (FileInfo file in search)
            {
                Assert.IsTrue(file.Exists);
            }

            Assert.IsTrue(directoryStarted > 0);
            Assert.AreEqual(directoryStarted, directoryEnded);
        }

        private void Search_DirectoryStarted(FileSearch source, FileSearchEventArgs eventArgs)
        {
            ++directoryStarted;
        }

        private void Search_DirectoryEnded(FileSearch source, FileSearchEventArgs eventArgs)
        {
            ++directoryEnded;
        }
    }
}
