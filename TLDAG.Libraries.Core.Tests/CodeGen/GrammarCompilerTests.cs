using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Libraries.Core.CodeGen;
using static TLDAG.Libraries.Core.IO.Directories;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    [TestClass]
    public class GrammarCompilerTests
    {
        [TestMethod]
        public void Test()
        {
            DirectoryInfo directory = GetDirectory();
            FileInfo source = new(Path.Combine(directory.FullName, "Grammar.g"));
            FileInfo dest = new(Path.Combine(directory.FullName, "Grammar.gz"));
            GrammarCompiler compiler = new();

            compiler.Compile(source, dest);
        }

        private static DirectoryInfo GetDirectory()
        {
            FileInfo assembly = new(typeof(GrammarCompilerTests).Assembly.Location);
            DirectoryInfo start = assembly.Directory ?? throw new FileNotFoundException();
            DirectoryInfo root = GetDirectoryOfFileAbove(start, "tldag-dotnet.sln");

            return new(Path.Combine(root.FullName, "TLDAG.Libraries.Core", "Resources"));
        }
    }
}
