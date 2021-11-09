using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Linq;
using System.Text;
using TLDAG.Core;
using TLDAG.Core.IO;
using TLDAG.Core.Xml;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class SourceStatements
        {
            public SourceInfo Source { get; }
            public int Statements { get; }

            public SourceStatements(SourceInfo source, int statements) { Source = source; Statements = statements; }
        }

        public class Valuator
        {
            public virtual SourceStatements Valuate(SourceInfo source)
                => Valuate(source, source.File.ReadAllText());

            public virtual SourceStatements Valuate(SourceInfo source, Encoding encoding)
                => Valuate(source, source.File.ReadAllText(encoding));

            public virtual SourceStatements Valuate(SourceInfo source, string text)
                => new(source, 0);
        }

        public class XmlValuator : Valuator
        {
            public override SourceStatements Valuate(SourceInfo source)
                => new(source, source.File.LoadXmlDocument().AllElements().Count());

            public override SourceStatements Valuate(SourceInfo source, Encoding _)
                => Valuate(source);

            public override SourceStatements Valuate(SourceInfo source, string text)
                => new(source, text.ToXmlDocument().AllElements().Count());
        }

        public class CSharpValuator : Valuator
        {
            public override SourceStatements Valuate(SourceInfo source, string text)
                => new(source, Walker.Walk(CSharpSyntaxTree.ParseText(text)));

            public class Walker : CSharpSyntaxVisitor
            {
                private int value;

                public static Walker Create() => new();

                public int Walk(SyntaxNode root)
                { value = 0; Visit(root); return value; }

                public static int Walk(SyntaxTree tree)
                    => tree.HasCompilationUnitRoot ? Create().Walk(tree.GetRoot()) : 0;
            }
        }

        public class ValuatorFactory : Factory
        {
            public ValuatorFactory(Options options) : base(options) { }

            public virtual Valuator GetValuator(SourceInfo source)
            {
                return source.Language switch
                {
                    "C#" => new CSharpValuator(),
                    "XML" => new XmlValuator(),
                    _ => new Valuator()
                };
            }
        }
    }
}