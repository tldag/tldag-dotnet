using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.NuGet
{
    // [TestClass]
    public class FindAllNugetConfigs
    {
        private long progressMillis = 0;

        // [TestMethod]
        public void TestMethod1()
        {
            DirectoryInfo directory = new(@"C:\");
            FileSearch files = new(directory, "NuGet.Config", true);

            files.DirectoryStarted += Files_DirectoryStarted;

            files.ToList().ForEach(file => { Debug.WriteLine(file.FullName); });
        }

        private void Files_DirectoryStarted(FileSearch source, FileSearchEventArgs eventArgs)
        {
            long millis = (long)Math.Round(eventArgs.Progress * 1000);

            if (millis > progressMillis)
            {
                progressMillis = millis;
                Debug.WriteLine($"{progressMillis} ‰");
            }
        }
    }
}
