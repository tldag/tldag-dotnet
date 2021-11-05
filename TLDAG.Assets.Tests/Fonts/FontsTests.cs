using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Core.Executing;
using TLDAG.Core.IO;
using TLDAG.Test;

namespace TLDAG.Assets.Tests.Fonts
{
    [TestClass]
    public class FontsTests : TestsBase
    {
        [TestMethod]
        public void MyTestMethod()
        {
            if (!Java.HasJava) return;
            if (!Files.TryFindOnPath("batik-ttf2svg-1.14.jar", out FileInfo jarFile)) return;

            DirectoryInfo directory = SolutionDirectory.CombineDirectory("TLDAG.Assets", "Fonts");
            FileInfo ttfFile = directory.Combine("tldag.ttf");
            FileInfo svgFile = directory.Combine("tldag.ttf.svg");

            if (svgFile.Exists)
                svgFile = GetTestDirectory().Combine("tldag.ttf.svg");

            string low = ((int)'A').ToString();
            string high = ((int)'Z').ToString();

            Java.Execute(jarFile, ttfFile.FullName, "-l", low, "-h", high, "-o", svgFile.FullName, "-testcard");
        }
    }
}
