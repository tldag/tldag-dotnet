using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using TLDAG.Build.Drawing;
using TLDAG.Core.Collections;
using TLDAG.Core.Executing;
using TLDAG.Core.Executing.Java;
using TLDAG.Core.IO;
using TLDAG.Test;

namespace TLDAG.Assets.Tests.Logos
{
    [TestClass]
    public class LogosTests : TestsBase
    {
        private static readonly IntSet Sizes = new(16, 24, 32, 48, 64, 96, 128);

        [TestMethod]
        public void Test()
        {
            if (!JavaExecutables.TryFind(out Executable java)) return;
            if (!Files.TryFindOnPath("batik-rasterizer-1.14.jar", out FileInfo jarFile)) return;

            DirectoryInfo directory = SolutionDirectory.CombineDirectory("TLDAG.Assets", "Logos");
            FileInfo svgFile = directory.Combine("TLDAG.svg");

            List<ExecutionResult> results = Sizes.Select(size => CreatePng(java, jarFile, svgFile, size)).ToList();

            results.ForEach(result => { Debug.WriteLine(result); });

            CreateIcon(directory);
        }

        private static ExecutionResult CreatePng(Executable java, FileInfo jarFile, FileInfo svgFile, int size)
        {
            string wh = size.ToString();
            FileInfo pngFile = svgFile.GetDirectory().Combine($"TLDAG_{size}.png");

            return ExecutionBuilder.Create(java)
                .AddArguments("-jar", jarFile.FullName)
                .AddArgument(svgFile.FullName)
                .AddArguments("-w", wh, "-h", wh)
                .AddArguments("-d", pngFile.FullName)
                .Build().Execute();
        }

        private static void CreateIcon(DirectoryInfo directory)
        {
            IEnumerable<Bitmap> bitmaps = Sizes
                .Select(size => directory.Combine($"TLDAG_{size}.png"))
                .Select(file => (Bitmap) Image.FromFile(file.FullName));
            byte[] iconData = Icons.Create(bitmaps);
            FileInfo iconFile = directory.Combine("TLDAG.ico");

            iconFile.WriteAllBytes(iconData);
        }
    }
}
