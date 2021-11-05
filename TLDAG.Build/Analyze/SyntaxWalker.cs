using Microsoft.CodeAnalysis;
using System;
using System.Collections;

namespace TLDAG.Build.Analyze
{
    public class UnknownSyntaxNodeEventArgs : EventArgs
    {
        public SyntaxNode Node { get; }

        public UnknownSyntaxNodeEventArgs(SyntaxNode node) { Node = node; }
    }

    public delegate void UnknownSyntaxNodeHandler(SyntaxWalker source, UnknownSyntaxNodeEventArgs e);

    public class SyntaxWalker
    {
        public event UnknownSyntaxNodeHandler? UnknownSyntaxNode;

        public virtual void Walk(SyntaxTree tree) { if (tree.HasCompilationUnitRoot) Walk(tree.GetRoot()); }

        protected virtual void Walk(SyntaxNode? node) { Unknown(node); }

        protected virtual void Walk(params object?[] args)
        {
            foreach (object? o in args)
            {
                if (o is null) continue;

                if (o is SyntaxNode node) { Walk(node); }
                else if (o is IEnumerable e) { Walk(e); }
            }
        }

        protected virtual void Walk(IEnumerable args)
        {
            foreach (object? o in args)
            {
                if (o is SyntaxNode node) { Walk(node); }
            }
        }

        protected virtual void Unknown(SyntaxNode? node)
        {
            if (node is null) return;

            UnknownSyntaxNode?.Invoke(this, new(node));
        }
    }
}
