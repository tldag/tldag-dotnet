using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using TLDAG.Core.IO;
using TLDAG.Core.Xml;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class Ncss
        {
            public SourceInfo Source { get; }
            public int Count { get; }

            public Ncss(SourceInfo source, int count) { Source = source; Count = count; }
        }

        public static Ncss GetNcss(SourceInfo source)
        {
            switch (source.Type)
            {
                case SourceType.Invalid: return new(source, 0);
                case SourceType.CSharp: return GetCsNcss(source);
                case SourceType.Xml: return GetXmlNcss(source);
            }

            return new(source, 0);
        }

        public static IEnumerable<Ncss> GetNcsses(string path)
            => GetSources(path).Select(GetNcss);

        public static Ncss GetXmlNcss(SourceInfo source)
            => new(source, GetXmlNcss(source.File.LoadXmlDocument().DocumentElement));

        private static int GetXmlNcss(XmlElement? element)
            => element is XmlElement ? 1 + element.GetChildElements().Select(GetXmlNcss).Sum() : 0;

        public static Ncss GetCsNcss(SourceInfo source)
        {
            string text = source.File.ReadAllText();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);

            if (tree.GetRoot() is not CompilationUnitSyntax root) return new(source, 0);

            return new(source, GetCsNcss(root.Members));
        }

        private static int GetCsNcss(IEnumerable<MemberDeclarationSyntax> members)
        {
            int ncss = 0;

            foreach (MemberDeclarationSyntax member in members)
            {
                if (member is PropertyDeclarationSyntax p) ncss += 1 + GetCsNcss(p.ExpressionBody);
                else if (member is IndexerDeclarationSyntax i) ncss += 1 + GetCsNcss(i.ExpressionBody);
                else if (member is BaseMethodDeclarationSyntax m) ncss += 1 + GetCsNcss(m.Body) + GetCsNcss(m.ExpressionBody);
                else if (member is TypeDeclarationSyntax t) ncss += 1 + GetCsNcss(t.Members);
                else if (member is EnumDeclarationSyntax e) ncss += 1 + GetCsNcss(e.Members);
                else if (member is NamespaceDeclarationSyntax n) ncss += 1 + GetCsNcss(n.Members);
                else ++ncss;

                // BaseFieldDeclarationSyntax ignored
            }

            return ncss;
        }

        private static int GetCsNcss(BlockSyntax? block)
        {
            if (block is null) return 0;

            int ncss = 0;

            foreach (StatementSyntax statement in block.Statements)
            {
                ncss += GetCsNcss(statement);
            }

            return ncss;
        }

        private static int GetCsNcss(SyntaxList<StatementSyntax> statements)
            => statements.Select(GetCsNcss).Sum();

        private static int GetCsNcss(StatementSyntax statement)
        {
            if (statement is BlockSyntax b) return GetCsNcss(b);
            else if (statement is CommonForEachStatementSyntax cfe) return 1 + GetCsNcss(cfe.Statement);
            else if (statement is DoStatementSyntax d) return 1 + GetCsNcss(d.Statement);
            else if (statement is ForStatementSyntax f) return 1 + GetCsNcss(f.Statement);
            else if (statement is IfStatementSyntax i) return 1 + GetCsNcss(i.Statement);
            else if (statement is SwitchStatementSyntax s) return 1 + GetCsNcss(s.Sections);
            else if (statement is TryStatementSyntax t) return 1 + GetCsNcss(t.Block) + t.Catches.Count;
            else if (statement is WhileStatementSyntax w) return 1 + GetCsNcss(w.Statement);
            return 1;
        }

        private static int GetCsNcss(ArrowExpressionClauseSyntax? arrow)
        {
            return arrow is null ? 0 : 1;
        }

        private static int GetCsNcss(SyntaxList<SwitchSectionSyntax> sections)
        {
            int ncss = 0;

            foreach (SwitchSectionSyntax section in sections)
            {
                ncss += 1 + GetCsNcss(section.Statements);
            }

            return ncss;
        }
    }
}
