using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.Executing;
using TLDAG.Core.IO;
using TLDAG.Drawing;
using TLDAG.Test;
using static TLDAG.Core.Executing.Java.Java;

namespace TLDAG.Assets.Tests.Logos
{
    [TestClass]
    public class LogosTests : TestsBase
    {
        private static readonly IntSet Sizes = new(16, 24, 32, 48, 64, 96, 128);

        [TestMethod]
        public void Test()
        {
            if (!HasJava()) return;
            if (!Files.TryFindOnPath("batik-rasterizer-1.14.jar", out FileInfo jarFile)) return;

            DirectoryInfo directory = SolutionDirectory.CombineDirectory("TLDAG.Assets", "Logos");
            FileInfo svgFile = directory.Combine("TLDAG.svg");
            FileInfo iconFile = directory.Combine("TLDAG.ico");

            List<ExecutionResult> results = Sizes.Select(size => CreatePng(jarFile, svgFile, size)).ToList();

            results.ForEach(result => { Debug.WriteLine(result); });

            Icons.Create(directory.EnumerateFiles("TLDAG_*.png"), iconFile);
        }

        private static ExecutionResult CreatePng(FileInfo jarFile, FileInfo svgFile, int size)
        {
            string wh = size.ToString();
            FileInfo pngFile = svgFile.GetDirectory().Combine($"TLDAG_{size}.png");

            return ExecuteJava(jarFile, svgFile.FullName, "-w", wh, "-h", wh, "-d", pngFile.FullName);
        }
    }
}
