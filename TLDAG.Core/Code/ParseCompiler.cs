using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public class ParseInitTerminals : IParseNodeVisitor
    {
        private int nextId = ParseTerminalNode.NextId;
        private readonly IntMap<ParseTerminalNode> terminals = new();

        public void Visit(ParseNode node)
        {
            if (node is ParseTerminalNode terminal)
            {
                if (terminal.Id == 0) { terminal.Id = nextId++; }
                terminals[terminal.Id] = terminal;
            }
        }

        public static IntMap<ParseTerminalNode> Init(ParseNode root)
        {
            ParseInitTerminals visitor = new();

            root.VisitDepthFirst(visitor);

            return visitor.terminals;
        }
    }

    public class ParseComputeFirst : IParseNodeVisitor
    {
        private readonly int EmptyId = ParseTerminalNode.EmptyId;
        private bool modified;

        public void Visit(ParseNode node)
        {
            if (node is ParseTerminalNode terminalNode) VisitTerminal(terminalNode);
            else if (node is ParseProductionNode productionNode) VisitProduction(productionNode);
        }

        private void VisitTerminal(ParseTerminalNode node)
        {
            AddToFirst(node, node.Id);
        }

        private void VisitProduction(ParseProductionNode node)
        {
            if (node.Count == 0) { AddToFirst(node, EmptyId); return; }

            IReadOnlyList<ParseNode> children = node.Children;

            AddToFirst(node, children[0].First);

            for (int i = 1, n = node.Count; i < n; ++i)
            {
                if (!children[i - 1].First.Contains(EmptyId)) return;
                AddToFirst(node, children[i].First);
            }

            AddToFirst(node, EmptyId);
        }

        private void AddToFirst(ParseNode node, int id) { if (node.AddToFirst(id)) modified = true; }
        private void AddToFirst(ParseNode node, IntSet ids) { if (node.AddToFirst(ids)) modified = true; }

        public static void Compute(ParseNode root)
        {
            ParseComputeFirst visitor = new();

            do
            {
                visitor.modified = false;
                root.VisitDepthFirst(visitor);
            }
            while (visitor.modified);
        }
    }

    public class ParseCompiler
    {
        private readonly ParseNode root;
        private readonly IntMap<ParseTerminalNode> terminals;

        public ParseCompiler(ParseNode root)
        {
            this.root = Extend(root);

            terminals = ParseInitTerminals.Init(this.root);
        }

        private static ParseProductionNode Extend(ParseNode root)
        {
            ParseNode[] children = { root, ParseTerminalNode.EndOfFile };

            return new(ExtendedGrammarRootName, children);
        }

        public static ParseCompiler Create(ParseNode root) => new(root);

        public ParseData Compile()
        {
            ParseComputeFirst.Compute(root);

            throw new NotImplementedException();
        }
    }
}
