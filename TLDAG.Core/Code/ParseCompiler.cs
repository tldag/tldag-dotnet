using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public class ParseElement : IEquatable<ParseElement>, IComparable<ParseElement>
    {
        public readonly ParseProductionNode Production;
        public readonly int Position;
        public readonly ParseTerminalNode Terminal;

        public ParseElement(ParseProductionNode production, int position, ParseTerminalNode terminal)
            { Production = production; Position = position; Terminal = terminal; }

        public override int GetHashCode() => Production.Id * 311 + Position * 31 + Terminal.Id;
        public override bool Equals(object? obj) => EqualsTo(obj as ParseElement);
        public bool Equals(ParseElement? other) => EqualsTo(other);

        private bool EqualsTo(ParseElement? other)
        {
            if (other is null) return false;
            if (Production.Id != other.Production.Id) return false;
            if (Position != other.Position) return false;
            return true;
        }

        public int CompareTo(ParseElement? other)
        {
            throw new NotImplementedException();
        }
    }

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

    public class ParseInitProductions : IParseNodeVisitor
    {
        private readonly IntMap<ParseProductionNode> productions = new();
        private int nextId = 1;

        public void Visit(ParseNode node)
        {
            if (node is ParseProductionNode productionNode)
            {
                if (productionNode.Id == 0) productionNode.Id = nextId++;
                productions[productionNode.Id] = productionNode;
            }
        }

        public static IntMap<ParseProductionNode> Init(ParseNode root)
        {
            ParseInitProductions visitor = new();

            root.VisitPreOrder(visitor);

            return visitor.productions;
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
        private readonly IntMap<ParseProductionNode> productions;

        public ParseCompiler(ParseNode root)
        {
            this.root = Extend(root);

            terminals = ParseInitTerminals.Init(this.root);
            productions = ParseInitProductions.Init(this.root);
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
