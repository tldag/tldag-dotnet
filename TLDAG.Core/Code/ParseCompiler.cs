using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public class ParseProductionMap : IntMap<ParseProductionNode> { }
    public class ParseTerminalMap : IntMap<ParseTerminalNode> { }

    public class ParseElement : IEquatable<ParseElement>, IComparable<ParseElement>
    {
        public class ElementKey : IEquatable<ElementKey>, IComparable<ElementKey>
        {
            public readonly int Production;
            public readonly int Position;
            public readonly int Terminal;

            private int? hash = null;

            public ElementKey(int production, int position, int terminal)
            { Production = production; Position = position; Terminal = terminal; }

            internal ElementKey(ParseProductionNode production, int position, ParseTerminalNode terminal)
                : this(production.Id, position, terminal.Id) { }

            public override bool Equals(object? obj) => EqualsTo(obj as ElementKey);
            public bool Equals(ElementKey? other) => EqualsTo(other);
            public bool EqualsTo(ElementKey? other) => throw NotYetImplemented();
            public override int GetHashCode() => hash ??= ComputeHashCode();
            public int CompareTo(ElementKey? other) => throw NotYetImplemented();

            private int ComputeHashCode() => Production << 21 + Position << 16 + Terminal;
        }

        public readonly ParseProductionNode Production;
        public int Position => Key.Position;
        public readonly ParseTerminalNode Terminal;
        public readonly ElementKey Key;

        public ParseElement(ParseProductionNode production, int position, ParseTerminalNode terminal)
            { Production = production; Terminal = terminal; Key = new(production, position, terminal); }

        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object? obj) => EqualsTo(obj as ParseElement);
        public bool Equals(ParseElement? other) => EqualsTo(other);
        private bool EqualsTo(ParseElement? other) => Key.EqualsTo(other?.Key);
        public int CompareTo(ParseElement? other) => Key.CompareTo(other?.Key);

        public static ParseElement Start(ParseProductionNode root)
        {
            ParseNodes children = root.Children;

            if (children.Count != 2) throw OutOfRange(nameof(root), children.Count, "Root must have 2 children.");

            if (children[1] is not ParseTerminalNode terminal)
                throw InvalidArgument(nameof(root), "Root's right child must be terminal.");

            return new(root, 0, terminal);
        }
    }

    public class ParseElementMap : SmartMap<ParseElement.ElementKey, ParseElement> { }

    public class ParseElements : SmartSet<ParseElement>, IEquatable<ParseElements>, IComparable<ParseElements>
    {
        public ParseElements(IEnumerable<ParseElement> elements) : base(elements) { }
        public ParseElements(ParseElement element) : base(element) { }

        public bool Equals(ParseElements? other) => throw NotYetImplemented();
        public int CompareTo(ParseElements? other) => throw NotYetImplemented();
    }

    public class ParseHulls : SmartMap<ParseElements, ParseElements> { }
    public class ParseFirsts : SmartMap<ParseNodes, IntSet> { }

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
        private readonly ParseTerminalMap terminals = new();

        public void Visit(ParseNode node)
        {
            if (node is ParseTerminalNode terminal)
            {
                if (terminal.Id == 0) { terminal.Id = nextId++; }
                terminals[terminal.Id] = terminal;
            }
        }

        public static ParseTerminalMap Init(ParseNode root)
        {
            ParseInitTerminals visitor = new();
            root.VisitDepthFirst(visitor);
            return visitor.terminals;
        }
    }

    public class ParseInitProductions : IParseNodeVisitor
    {
        private readonly ParseProductionMap productions = new();
        private int nextId = 1;

        public void Visit(ParseNode node)
        {
            if (node is ParseProductionNode productionNode)
            {
                if (productionNode.Id == 0) productionNode.Id = nextId++;
                productions[productionNode.Id] = productionNode;
            }
        }

        public static ParseProductionMap Init(ParseNode root)
        {
            ParseInitProductions visitor = new();
            root.VisitPreOrder(visitor);
            return visitor.productions;
        }
    }

    public class ParseCompiler
    {
        private readonly ParseProductionNode root;
        private readonly ParseTerminalMap terminals;
        private readonly ParseProductionMap productions;

        private readonly int[] terminalIds;

        private readonly ParseElementMap elements = new();
        private readonly ParseHulls hulls = new();
        private readonly ParseFirsts firsts = new();

        public ParseCompiler(ParseNode rawRoot)
        {
            root = Extend(rawRoot);

            terminals = ParseInitTerminals.Init(root);
            productions = ParseInitProductions.Init(root);

            terminalIds = terminals.Keys.ToArray();
        }

        public static ParseCompiler Create(ParseNode root) => new(root);

        private static ParseProductionNode Extend(ParseNode root)
            => new(ExtendedGrammarRootName, new(new ParseNode[] { root, ParseTerminalNode.EndOfFile }));

        public ParseData Compile()
        {
            SortedSet<ParseElements> elementSets = ElementSets();

            throw NotYetImplemented();
        }

        private IntSet First(ParseNodes nodes) => firsts[nodes] ??= ComputeFirst(nodes);

        private IntSet ComputeFirst(ParseNodes nodes) { throw NotYetImplemented(); }

        private ParseElements Hull(ParseElements elements) => hulls[elements] ??= ComputeHull(elements);

        private ParseElements ComputeHull(ParseElements elements)
        {
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
