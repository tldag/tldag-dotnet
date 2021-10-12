using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Parse
    {
        internal abstract class Visitor : Code.Parse.IVisitor
        {
            public abstract void Visit(Code.Parse.INode node);
        }

        internal abstract class Node : Code.Parse.INode, IEquatable<Node>, IComparable<Node>
        {
            public uint Id { get; internal set; }
            public abstract UIntSet First { get; }

            public Node(uint id) { Id = id; }

            public abstract V VisitDepthFirst<V>(V visitor) where V : Code.Parse.IVisitor;
            public abstract V VisitPreOrder<V>(V visitor) where V : Code.Parse.IVisitor;
            protected virtual V Visit<V>(V visitor) where V : Code.Parse.IVisitor { visitor.Visit(this); return visitor; }

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Node? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Node? other) => throw NotYetImplemented();
        }

        internal class Nodes : IReadOnlyList<Node>, IEquatable<Nodes>, IComparable<Nodes>
        {
            private readonly List<Node> nodes;

            public int Count => nodes.Count;
            
            public UIntSet First => throw NotYetImplemented();

            public Nodes(IEnumerable<Node> nodes) { this.nodes = nodes.ToList(); }

            public Nodes SubNodes(int start) => throw NotYetImplemented();

            public Node this[int index] => nodes[index];

            public IEnumerator<Node> GetEnumerator() => nodes.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => nodes.GetEnumerator();

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Nodes? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Nodes? other) => throw NotYetImplemented();
        }

        internal class Terminal : Node, Code.Parse.ITerminal, IEquatable<Terminal>, IComparable<Terminal>
        {
            public const uint EndOfFileId = 1;
            public const uint EmptyId = 2;
            public const uint NextId = 3;

            public static readonly Terminal EndOfFile = new(EndOfFileId, EndOfFileName);
            public static readonly Terminal Empty = new(EmptyId, EmptyNodeName);

            public string Name { get; }

            private UIntSet? first = null;
            public override UIntSet First => first ??= ComputeFirst();

            public Terminal(string name) : this(0, name) { }
            private Terminal(uint id, string name) : base(id) { Name = name; }

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Terminal? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Terminal? other) => throw NotYetImplemented();

            public override V VisitDepthFirst<V>(V visitor) { return Visit(visitor); }
            public override V VisitPreOrder<V>(V visitor) { return Visit(visitor); }

            private UIntSet ComputeFirst() => throw NotYetImplemented();
        }

        internal class Production : Node, Code.Parse.IProduction, IEquatable<Production>, IComparable<Production>
        {
            public string Name { get; }
            public IEnumerable<Code.Parse.INode> Children => children;
            public int Count => children.Count;

            public override UIntSet First => children.First;

            internal readonly Nodes children;

            public Production(string name, Nodes children) : base(0) { Name = name; this.children = children; }

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Production? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Production? other) => throw NotYetImplemented();

            public override V VisitDepthFirst<V>(V visitor)
            {
                foreach (Node node in children) node.VisitDepthFirst(visitor);
                return Visit(visitor);
            }

            public override V VisitPreOrder<V>(V visitor)
            {
                Visit(visitor);
                foreach (Node node in children) node.VisitPreOrder(visitor);
                return visitor;
            }
        }

        internal class Choose : Node, IEquatable<Choose>, IComparable<Choose>
        {
            public readonly Node Left;
            public readonly Node Right;

            private UIntSet? first = null;
            public override UIntSet First => first ??= ComputeFirst();

            public Choose(Node left, Node right) : base(0) { Left = left; Right = right; }

            public override bool Equals(object? obj) => throw NotYetImplemented();
            public bool Equals(Choose? other) => throw NotYetImplemented();
            public override int GetHashCode() => throw NotYetImplemented();
            public int CompareTo(Choose? other) => throw NotYetImplemented();

            public override V VisitDepthFirst<V>(V visitor) { throw NotYetImplemented(); }
            public override V VisitPreOrder<V>(V visitor) { throw NotYetImplemented(); }

            private UIntSet ComputeFirst() => throw NotYetImplemented();
        }

        internal class Builder
        {
            private readonly StringSet terminalNames;
            private readonly SmartMap<string, Terminal> terminals = new();
            private readonly SmartMap<string, Production> productions = new();
            private readonly Stack<Node> stack = new();

            public Builder(IEnumerable<string> tokenNames)
            {
                terminalNames = new(tokenNames);
            }

            public static Builder Create(IEnumerable<string> tokenNames) => new(tokenNames);

            public Production Build()
            {
                ValidateStack(1, 1);

                if (stack.Peek() is not Production root) throw new InvalidOperationException();
                stack.Pop();

                return root;
            }

            public void Terminal(string name) { ValidateTerminalName(name); stack.Push(GetTerminal(name)); }
            public void Empty() { stack.Push(Parse.Terminal.Empty); }

            public void Production(string name, int count)
            {
                ValidateStack(count);
                ValidateProductionName(name);

                if (count == 0) { Empty(); Production(name, 1); return; }

                Node[] children = new Node[count];

                for (int i = count - 1; i >= 0; --i) children[i] = stack.Pop();

                Production production = new(name, new(children));

                stack.Push(production);
                Add(production);
            }

            public void Production(string name) { throw NotYetImplemented(); }
            public void Choose() { throw NotYetImplemented(); }

            private Terminal GetTerminal(string name) {  throw NotYetImplemented();  }

            private void ValidateStack(int min, int max = int.MaxValue)
            {
                if (stack.Count < min) throw new InvalidOperationException();
                if (stack.Count > max) throw new InvalidOperationException();
            }

            private void ValidateTerminalName(string name)
            {
                if (ReservedTerminalNames.Contains(name)) throw new ArgumentException();
                if (ReservedTokenNames.Contains(name)) return;
                if (!terminalNames.Contains(name)) throw new ArgumentException();
            }

            private void ValidateProductionName(string name)
            {
                if (ReservedProductionNames.Contains(name)) throw new ArgumentException();
                if (!ProductionNameRegex.IsMatch(name)) throw new ArgumentException();
                if (productions.ContainsKey(name)) throw new ArgumentException();
            }

            private void Add(Production production)
            {
                productions[production.Name] = production;
            }
        }
    }
}