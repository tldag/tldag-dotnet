using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TLDAG.Core.IO;

namespace TLDAG.Build.Tests
{
    [TestClass]
    public class CompileTests
    {
        [TestMethod]
        public void TestParse()
        {
            Compile
                .GetCSharpFiles(GetListFile())
                .Select(file => File.ReadAllText(file.FullName))
                .Select(text => CSharpSyntaxTree.ParseText(text))
                .Select(tree => tree.GetCompilationUnitRoot())
                .ToList();
        }

        [TestMethod]
        public void TestGetCompileFiles()
        {
            FileInfo[] files = Compile.GetCompileFiles(GetListFile()).ToArray();

            Assert.AreEqual(5, files.Length);
        }

#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif

        private static FileInfo GetListFile()
        {
            DirectoryInfo root = Directories.GetDirectoryOfFileAbove(".", "tldag-dotnet.sln");

            return root.Combine("TLDAG.Build", "obj", Configuration, "net5.0", "Compile.lst");
        }
    }
}
