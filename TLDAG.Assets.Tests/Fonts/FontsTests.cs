using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Core.IO;
using TLDAG.Test;
using static TLDAG.Core.Executing.Java.Java;

namespace TLDAG.Assets.Tests.Fonts
{
    [TestClass]
    public class FontsTests : TestsBase
    {
        [TestMethod]
        public void MyTestMethod()
        {
            if (!HasJava()) return;
            if (!Files.TryFindOnPath("batik-ttf2svg-1.14.jar", out FileInfo jarFile)) return;

            DirectoryInfo directory = SolutionDirectory.CombineDirectory("TLDAG.Assets", "Fonts");
            FileInfo ttfFile = directory.Combine("tldag.ttf");
            FileInfo svgFile = directory.Combine("tldag.ttf.svg");

            if (svgFile.Exists)
                svgFile = GetTestDirectory(true).Combine("tldag.ttf.svg");

            string low = ((int)'A').ToString();
            string high = ((int)'Z').ToString();

            ExecuteJava(jarFile, ttfFile.FullName, "-l", low, "-h", high, "-o", svgFile.FullName, "-testcard");
        }
    }
}
