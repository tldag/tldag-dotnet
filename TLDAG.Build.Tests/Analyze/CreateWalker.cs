using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using TLDAG.Core.IO;
using TLDAG.Test;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TLDAG.Build.Tests.Analyze
{
    [TestClass]
    public class CreateWalker : TestsBase
    {
        [TestMethod]
        public void Create()
        {
            FileInfo file = GetTestDirectory().Combine("CSharpNodeWalker.cs");
            using FileStream stream = file.Open(FileMode.Create);
            using StreamWriter writer = new(stream, Encoding.UTF8);

            CreateCompilationUnit().NormalizeWhitespace().WriteTo(writer);
        }

        private CompilationUnitSyntax CreateCompilationUnit()
        {
            return CompilationUnit()
                .AddUsings(CreateUsings())
                .AddMembers(CreateNamespaces());
        }

        private UsingDirectiveSyntax[] CreateUsings()
        {
            return new UsingDirectiveSyntax[]
            {

            };
        }

        private MemberDeclarationSyntax[] CreateNamespaces()
            => new[] { CreateNamespace() };

        private NamespaceDeclarationSyntax CreateNamespace()
        {
            return NamespaceDeclaration(ParseName("TLDAG.Build.Analyze"));
        }
    }
}
