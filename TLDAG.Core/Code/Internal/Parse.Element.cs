using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Parse
    {
        internal class ElementKey : IEquatable<ElementKey>, IComparable<ElementKey>
        {
            public readonly uint Production;
            public readonly int Position;
            public readonly uint Terminal;

            private int? hash = null;

            public ElementKey(uint production, int position, uint terminal)
            { Production = production; Position = position; Terminal = terminal; }

            internal ElementKey(Production production, int position, Terminal terminal)
                : this(production.Id, position, terminal.Id) { }

            public override bool Equals(object? obj) => EqualsTo(obj as ElementKey);
            public bool Equals(ElementKey? other) => EqualsTo(other);
            public bool EqualsTo(ElementKey? other) => throw NotYetImplemented();
            public override int GetHashCode() => hash ??= ComputeHashCode();
            public int CompareTo(ElementKey? other) => throw NotYetImplemented();

            private int ComputeHashCode()
                => Production.GetHashCode() << 21 + Position << 16 + Terminal.GetHashCode();
        }

        internal class Element : IEquatable<Element>, IComparable<Element>
        {
            internal class GetProductions
            {
                private static void Visit(Node node, SortedSet<Production> productions)
                {
                    if (node is Terminal) return;
                    else if (node is Production production) productions.Add(production);
                    else if (node is Choose choose) { Visit(choose.Left, productions); Visit(choose.Right, productions); }
                }

                public static IEnumerable<Production> Get(Element element)
                {
                    Production production = element.Production;
                    if (element.Position == production.Count) return Array.Empty<Production>();

                    Node node = production.children[element.Position];
                    if (node is Terminal) return Array.Empty<Production>();

                    SortedSet<Production> productions = new();
                    Visit(node, productions);

                    return productions;
                }
            }

            public readonly Production Production;
            public int Position => Key.Position;
            public readonly Terminal Terminal;
            public readonly ElementKey Key;

            private IEnumerable<Production>? productions = null;
            public IEnumerable<Production> Productions => productions ??= GetProductions.Get(this);

            public Element(Production production, int position, Terminal terminal)
            { Production = production; Terminal = terminal; Key = new(production, position, terminal); }

            public override int GetHashCode() => Key.GetHashCode();
            public override bool Equals(object? obj) => EqualsTo(obj as Element);
            public bool Equals(Element? other) => EqualsTo(other);
            private bool EqualsTo(Element? other) => Key.EqualsTo(other?.Key);
            public int CompareTo(Element? other) => Key.CompareTo(other?.Key);

            public IEnumerable<Nodes> Follow(Production production, Terminal terminal)
            {
                throw NotYetImplemented();
            }

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
    }
}