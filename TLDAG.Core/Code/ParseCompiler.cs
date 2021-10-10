using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public class ParseElement : IEquatable<ParseElement>, IComparable<ParseElement>
    {
        public readonly int Production;
        public readonly int Position;
        public readonly int Terminal;

        private int? hash = null;

        public ParseElement(int production, int position, int terminal)
            { Production = production; Position = position; Terminal = terminal; }

        private int ComputeHashCode() => Production << 21 + Position << 16 + Terminal;
        public override int GetHashCode() => hash ??= ComputeHashCode();
        public override bool Equals(object? obj) => EqualsTo(obj as ParseElement);
        public bool Equals(ParseElement? other) => EqualsTo(other);

        private bool EqualsTo(ParseElement? other)
        {
            if (other is null) return false;
            if (Production != other.Production) return false;
            if (Position != other.Position) return false;
            return true;
        }

        public int CompareTo(ParseElement? other)
        {
            throw NotYetImplemented();
        }

        public static ParseElement Start(ParseProductionNode root)
        {
            ParseNodes children = root.Children;

            if (children.Count != 2) throw OutOfRange(nameof(root), children.Count, "Root must have 2 children.");

            if (children[1] is not ParseTerminalNode terminal)
                throw InvalidArgument(nameof(root), "Root's right child must be terminal.");

            return new(root.Id, 0, terminal.Id);
        }
    }

    public class ParseElements : SmartSet<ParseElement>, IEquatable<ParseElements>
    {
        public ParseElements(IEnumerable<ParseElement> elements) : base(elements) { }
        public ParseElements(ParseElement element) : base(element) { }

        public bool Equals(ParseElements? other) => throw NotYetImplemented();
    }

    public class ParseElementsCollector
    {
        private readonly SortedSet<ParseElement> elements = new();

        public ParseElement[] Current => elements.ToArray();

        public bool Add(ParseElement element) { throw NotYetImplemented(); }
        public ParseElements Build() { throw NotYetImplemented(); }
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

    public class ParseCompiler
    {
        private readonly ParseProductionNode root;
        private readonly IntMap<ParseTerminalNode> terminals;
        private readonly IntMap<ParseProductionNode> productions;

        private readonly int[] terminalIds;

        private readonly SmartMap<ParseElements, ParseElements> hulls = new();
        private readonly SmartMap<ParseNodes, IntSet> firsts = new();

        public ParseCompiler(ParseNode root)
        {
            this.root = Extend(root);

            terminals = ParseInitTerminals.Init(this.root);
            productions = ParseInitProductions.Init(this.root);

            terminalIds = terminals.Keys.ToArray();
        }

        private static ParseProductionNode Extend(ParseNode root)
        {
            ParseNode[] children = { root, ParseTerminalNode.EndOfFile };

            return new(ExtendedGrammarRootName, new(children));
        }

        public static ParseCompiler Create(ParseNode root) => new(root);

        public ParseData Compile()
        {
            SortedSet<ParseElements> elementSets = ElementSets();

            throw NotYetImplemented();
        }

        private IntSet First(ParseNodes nodes)
        {
            throw NotYetImplemented();
        }

        private ParseElements Hull(ParseElements elements)
        {
            ParseElements? hull = hulls[elements];

            if (hull is not null) return hull;

            ParseElementsCollector collector = new();
            bool modified;

            do
            {
                ParseElement[] current = collector.Current;

                modified = false;

                throw NotYetImplemented();
            }
            while (modified);

            return collector.Build();
        }

        private ParseElements Transition(ParseElements elements, int terminalId)
        {
            throw NotYetImplemented();
        }

        private SortedSet<ParseElements> ElementSets()
        {
            SortedSet<ParseElements> elementSets = new();
            
            bool modified;

            elementSets.Add(Hull(new(ParseElement.Start(root))));

            do
            {
                ParseElements[] current = elementSets.ToArray();

                modified = false;

                foreach (ParseElements elements in current)
                {
                    foreach (int terminalId in terminalIds)
                    {
                        ParseElements transition = Transition(elements, terminalId);

                        if (transition.Count > 0 && !elementSets.Contains(transition))
                        {
                            elementSets.Add(transition);
                            modified = true;
                        }
                    }
                }
            }
            while (modified);

            return elementSets;
        }
    }
}
