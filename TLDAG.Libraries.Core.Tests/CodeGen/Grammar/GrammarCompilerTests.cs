using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TLDAG.Libraries.Core.CodeGen.Grammar;
using static TLDAG.Libraries.Core.IO.Directories;

namespace TLDAG.Libraries.Core.Tests.CodeGen.Grammar
{
    [TestClass]
    public class GrammarCompilerTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            DirectoryInfo directory = GetDirectory();
            FileInfo source = new(Path.Combine(directory.FullName, "Grammar.g"));
            FileInfo dest = new(Path.Combine(directory.FullName, "Grammar.gz"));
            GrammarCompiler compiler = CreateGrammarCompiler();

            compiler.Compile(source, dest);
        }

        private static DirectoryInfo GetDirectory()
        {
            FileInfo assembly = new(typeof(GrammarCompilerTests).Assembly.Location);
            DirectoryInfo start = assembly.Directory ?? throw new FileNotFoundException();
            DirectoryInfo root = GetDirectoryOfFileAbove(start, "tldag-dotnet.sln");

            return new(Path.Combine(root.FullName, "TLDAG.Libraries.Core", "Resources"));
        }

        private static GrammarCompiler CreateGrammarCompiler()
        {
#if DEBUG
            Gramm.Parser parser = new();
            Gramm.Compiler compiler = new(parser);

            return new(compiler);
#else
            return new();
#endif
            throw new NotImplementedException();
        }
    }
}
