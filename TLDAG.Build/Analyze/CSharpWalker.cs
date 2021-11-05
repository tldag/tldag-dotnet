using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TLDAG.Core;
using TLDAG.Core.IO;

namespace TLDAG.Build.Analyze
{
    public class CSharpWalker : SyntaxWalker
    {
        private static MethodInfo GetMethod(string name)
            => Contract.Arg.As<MethodInfo>(typeof(CSharpWalker).GetMethod(name), name);

        private static readonly Dictionary<SyntaxKind, MethodInfo> KindMethods = new()
        {
            { SyntaxKind.Argument, GetMethod("Argument") }
        };

        public static CSharpWalker Create() => new();

        public void Walk(FileInfo file) { Walk(CSharpSyntaxTree.ParseText(file.ReadAllText())); }

        protected override void Walk(SyntaxNode? node)
        {
            if (node is CSharpSyntaxNode cSharpNode)
                Invoke(cSharpNode);
            else
                base.Walk(node);
        }

        public virtual void Argument(ArgumentSyntax n) { Walk(n.NameColon, n.Expression); }

        private void Invoke(CSharpSyntaxNode node)
        {
            if (KindMethods.TryGetValue(node.Kind(), out MethodInfo? method))
            {
                method?.Invoke(this, new object[] { node });
            }
            else
            {
                Unknown(node);
            }
        }
    }
}
