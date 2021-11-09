using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.Build.Valuating
{
    public class CSharpValuator : Valuator
    {
        public static CSharpValuator Instance { get; } = new();

        protected CSharpValuator() { }

        public override int Valuate(string source)
            => Walker.Walk(CSharpSyntaxTree.ParseText(source));

        public class Walker : CSharpSyntaxVisitor
        {
            private int value;

            public static Walker Create() => new();

            public int Walk(SyntaxNode root)
                { value = 0; Visit(root); return value;}

            public static int Walk(SyntaxTree tree)
                => tree.HasCompilationUnitRoot ? Create().Walk(tree.GetRoot()) : 0;
        }
    }
}