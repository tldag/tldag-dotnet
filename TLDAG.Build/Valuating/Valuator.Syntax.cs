using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TLDAG.Core;
using static TLDAG.Build.Resources.ValuationResources;

namespace TLDAG.Build.Valuating
{
    public class SyntaxValuator : Valuator
    {
        public virtual int Tree(SyntaxTree tree)
            => tree.HasCompilationUnitRoot ? Node(tree.GetRoot()) : 0;

        public virtual int Node(SyntaxNode? node) => Unknown(node);

        public virtual int Unknown(SyntaxNode? node)
        {
            if (node is not null)
                Debug.WriteLine(NoValuationForSyntax.Format(node.GetType()));

            return 0;
        }

        public virtual int Valuate<N>(SyntaxNode node, Func<N, int> valuate) where N : notnull, SyntaxNode
            => node is N n ? valuate(n) : 0;

        public virtual int Nodes<N>(IEnumerable<N> nodes) where N : notnull, SyntaxNode
            => nodes.Select(Node).Sum();
    }
}