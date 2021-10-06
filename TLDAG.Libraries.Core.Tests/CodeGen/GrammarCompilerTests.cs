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
        private readonly DirectoryInfo directory = GetDirectory();
        private readonly FileInfo source;
        private readonly FileInfo dest;

        public GrammarCompilerTests()
        {
            source = new(Path.Combine(directory.FullName, "Grammar.g"));
            dest = new(Path.Combine(directory.FullName, "Grammar.gz"));
        }

#if DEBUG
        [TestMethod]
        public void TestGrammarDevCompiler()
        {
            ParseCompiler compiler = Grammar.CreateDevCompiler();

            compiler.Compile();
        }
#endif

        [TestMethod]
        public void CreateGrammarGz()
        {
            if (dest.Exists) return;

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
