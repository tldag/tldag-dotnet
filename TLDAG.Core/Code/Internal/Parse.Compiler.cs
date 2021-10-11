using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Parse
    {
        internal class ProductionMap : IntMap<Production> { }
        internal class TerminalMap : IntMap<Terminal> { }
        internal class ElementMap : SmartMap<Element.ElementKey, Element> { }
        internal class Hulls : SmartMap<Elements, Elements> { }
        internal class Firsts : SmartMap<Nodes, IntSet> { }

        internal class Element : IEquatable<Element>, IComparable<Element>
        {
            internal class ElementKey : IEquatable<ElementKey>, IComparable<ElementKey>
            {
                public readonly int Production;
                public readonly int Position;
                public readonly int Terminal;

                private int? hash = null;

                public ElementKey(int production, int position, int terminal)
                { Production = production; Position = position; Terminal = terminal; }

                internal ElementKey(Production production, int position, Terminal terminal)
                    : this(production.Id, position, terminal.Id) { }

                public override bool Equals(object? obj) => EqualsTo(obj as ElementKey);
                public bool Equals(ElementKey? other) => EqualsTo(other);
                public bool EqualsTo(ElementKey? other) => throw NotYetImplemented();
                public override int GetHashCode() => hash ??= ComputeHashCode();
                public int CompareTo(ElementKey? other) => throw NotYetImplemented();

                private int ComputeHashCode() => Production << 21 + Position << 16 + Terminal;
            }

            internal class GetProductions : Visitor
            {
                public override void Visit(Code.Parse.INode node) => throw NotYetImplemented();

                public static Production[] Get(Element element)
                {
                    if (element.Position == element.Production.Count) return Array.Empty<Production>();

                    throw NotYetImplemented();
                }
            }

            public readonly Production Production;
            public int Position => Key.Position;
            public readonly Terminal Terminal;
            public readonly ElementKey Key;

            private Production[]? productions = null;
            public IEnumerable<Production> Productions => productions ??= GetProductions.Get(this);

            public Element(Production production, int position, Terminal terminal)
            { Production = production; Terminal = terminal; Key = new(production, position, terminal); }

            public override int GetHashCode() => Key.GetHashCode();
            public override bool Equals(object? obj) => EqualsTo(obj as Element);
            public bool Equals(Element? other) => EqualsTo(other);
            private bool EqualsTo(Element? other) => Key.EqualsTo(other?.Key);
            public int CompareTo(Element? other) => Key.CompareTo(other?.Key);

            public static Element Start(Production root)
            {
                Nodes children = root.children;

                if (children.Count != 2) throw OutOfRange(nameof(root), children.Count, "Root must have 2 children.");

                if (children[1] is not Terminal terminal)
                    throw InvalidArgument(nameof(root), "Root's right child must be terminal.");

                return new(root, 0, terminal);
            }
        }

        internal class Elements : SmartSet<Element>, IEquatable<Elements>, IComparable<Elements>
        {
            public Elements(IEnumerable<Element> elements) : base(elements) { }
            public Elements(Element element) : base(element) { }

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Elements? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Elements? other) => throw NotYetImplemented();
        }

        internal class ElementCollector
        {
            private readonly SortedSet<Element> elements;

            public ElementCollector(Elements elements)
            {
                this.elements = new(elements);
            }

            public Element[] Current => elements.ToArray();
            public bool Add(Element element) { throw NotYetImplemented(); }
            public Elements Build() { throw NotYetImplemented(); }
        }

        internal class InitTerminals : Visitor
        {
            private int nextId = Terminal.NextId;
            private readonly TerminalMap terminals = new();

            public override void Visit(Code.Parse.INode node)
            {
                if (node is Terminal terminal)
                {
                    if (terminal.Id == 0) { terminal.Id = nextId++; }
                    terminals[terminal.Id] = terminal;
                }
            }

            public static TerminalMap Init(Node root)
            {
                InitTerminals visitor = new();
                root.VisitDepthFirst(visitor);
                return visitor.terminals;
            }
        }

        internal class InitProductions : Visitor
        {
            private readonly ProductionMap productions = new();
            private int nextId = 1;

            public override void Visit(Code.Parse.INode node)
            {
                if (node is Production production)
                {
                    if (production.Id == 0) production.Id = nextId++;
                    productions[production.Id] = production;
                }
            }

            public static ProductionMap Init(Node root)
            {
                InitProductions visitor = new();
                root.VisitPreOrder(visitor);
                return visitor.productions;
            }
        }

        internal class Compiler
        {
            private readonly Production root;
            private readonly TerminalMap terminals;
            private readonly ProductionMap productions;

            private IntSet terminalIds;

            private readonly ElementMap elements = new();
            private readonly Hulls hulls = new();
            private readonly Firsts firsts = new();

            public Compiler(Code.Parse.INode node)
            {
                root = Extend(node);
                terminals = InitTerminals.Init(root);
                productions = InitProductions.Init(root);

                terminalIds = new(terminals.Keys);
            }

            private static Production Extend(Code.Parse.INode node)
            {
                Node root = node as Node ?? throw InvalidArgument(nameof(node), $"{node.GetType()} not supported.");
                return new(ExtendedGrammarRootName, new(new Node[] { root, Terminal.EndOfFile }));
            }
            
            public Code.Parse.IData Compile()
            {
                SortedSet<Elements> elementSets = CreateElementSets();

                throw NotYetImplemented();
            }

            private Elements GetHull(Elements elements) => hulls[elements] ??= ComputeHull(elements);

            private Elements ComputeHull(Elements elements)
            {
                ElementCollector collector = new(elements);
                bool modified;

                do
                {
                    Element[] current = collector.Current;

                    modified = false;

                    foreach (Element element in current)
                    {
                        foreach (Production production in element.Productions)
                        {

                        }
                    }

                    throw NotYetImplemented();
                }
                while (modified);

                return collector.Build();
            }

            private IntSet GetFirst(Nodes nodes) => firsts[nodes] ??= ComputeFirst(nodes);

            private IntSet ComputeFirst(Nodes nodes) { throw NotYetImplemented(); }

            private SortedSet<Elements> CreateElementSets()
            {
                SortedSet<Elements> elementSets = new();

                bool modified;

                elementSets.Add(GetHull(new(Element.Start(root))));

                do
                {
                    Elements[] current = elementSets.ToArray();

                    modified = false;

                    foreach (Elements elements in current)
                    {
                        foreach (int terminalId in terminalIds)
                        {
                            Elements transition = GetTransition(elements, terminalId);

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

            private Elements GetTransition(Elements elements, int terminalId)
            {
                throw NotYetImplemented();
            }

        }
    }
}