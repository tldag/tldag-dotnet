using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public interface IParseNodeVisitor
    {
        public void Visit(ParseNode node);
    }

    public abstract class ParseNode : IEquatable<ParseNode>, IComparable<ParseNode>
    {
        public int Id { get; internal set; }

        public ParseNode(int id) { Id = id; }

        public override bool Equals(object? obj) => throw NotYetImplemented();
        public bool Equals(ParseNode? other) => throw NotYetImplemented();
        public override int GetHashCode() => throw NotYetImplemented();
        public int CompareTo(ParseNode? other) => throw NotYetImplemented();

        public virtual V VisitDepthFirst<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        public virtual V VisitPreOrder<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        protected virtual V Visit<V>(V visitor) where V : IParseNodeVisitor { visitor.Visit(this); return visitor; }

    }

    public class ParseNodes : IReadOnlyList<ParseNode>, IEquatable<ParseNodes>, IComparable<ParseNodes>
    {
        private List<ParseNode> nodes;

        public int Count => nodes.Count;

        public ParseNodes(IEnumerable<ParseNode> nodes) { this.nodes = nodes.ToList(); }

        public override bool Equals(object? obj) => throw NotYetImplemented();
        public bool Equals(ParseNodes? other) => throw NotYetImplemented();
        public override int GetHashCode() => throw NotYetImplemented();
        public int CompareTo(ParseNodes? other) => throw NotYetImplemented();

        public ParseNode this[int index] => nodes[index];
        public IEnumerator<ParseNode> GetEnumerator() => nodes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => nodes.GetEnumerator();

        public ParseNodes SubNodes(int start) => throw NotYetImplemented();
    }

    public class ParseTerminalNode : ParseNode, IEquatable<ParseTerminalNode>, IComparable<ParseTerminalNode>
    {
        public readonly string Name;

        public ParseTerminalNode(string name) : this(0, name) { }
        private ParseTerminalNode(int id, string name) : base(id) { Name = name; }

        public override bool Equals(object? obj) => throw NotYetImplemented();
        public bool Equals(ParseTerminalNode? other) => throw NotYetImplemented();
        public override int GetHashCode() => throw NotYetImplemented();
        public int CompareTo(ParseTerminalNode? other) => throw NotYetImplemented();

        public const int EndOfFileId = 1;
        public const int EmptyId = 2;
        public const int NextId = 3;

        public static readonly ParseTerminalNode EndOfFile = new(EndOfFileId, EndOfFileName);
        public static readonly ParseTerminalNode Empty = new(EmptyId, EmptyNodeName);
    }

    public class ParseProductionNode : ParseNode, IEquatable<ParseProductionNode>, IComparable<ParseProductionNode>
    {
        public readonly string Name;
        public ParseNodes Children;
        public int Count => Children.Count;

        public ParseProductionNode(string name, ParseNodes children) : base(0) { Name = name; Children = children; }

        public override bool Equals(object? obj) => throw NotYetImplemented();
        public bool Equals(ParseProductionNode? other) => throw NotYetImplemented();
        public override int GetHashCode() => throw NotYetImplemented();
        public int CompareTo(ParseProductionNode? other) => throw NotYetImplemented();

        public override V VisitDepthFirst<V>(V visitor)
        {
            foreach (ParseNode node in Children) node.VisitDepthFirst(visitor);
            return Visit(visitor);
        }

        public override V VisitPreOrder<V>(V visitor)
        {
            Visit(visitor);
            foreach (ParseNode node in Children) node.VisitPreOrder(visitor);
            return visitor;
        }
    }

    public class ParseChooseNode : ParseNode, IEquatable<ParseChooseNode>, IComparable<ParseChooseNode>
    {
        public readonly ParseNode Left;
        public readonly ParseNode Right;

        public ParseChooseNode(ParseNode left, ParseNode right) : base(0) { Left = left; Right = right; }

        public override bool Equals(object? obj) => throw NotYetImplemented();
        public bool Equals(ParseChooseNode? other) => throw NotYetImplemented();
        public override int GetHashCode() => throw NotYetImplemented();
        public int CompareTo(ParseChooseNode? other) => throw NotYetImplemented();

        public override V VisitDepthFirst<V>(V visitor) { throw NotYetImplemented(); }
        public override V VisitPreOrder<V>(V visitor) { throw NotYetImplemented(); }
    }

    internal class ProductionBuilder
    {
        private readonly StringSet terminalNames;
        private readonly SmartMap<string, ParseTerminalNode> terminals = new();
        private readonly SmartMap<string, ParseProductionNode> productions = new();

        private readonly Stack<ParseNode> stack = new();

        public ProductionBuilder(IEnumerable<string> tokenNames)
        {
            terminalNames = new(tokenNames);
        }

        public static ProductionBuilder Create(IEnumerable<string> tokenNames) => new(tokenNames);

        public ParseProductionNode Build()
        {
            ValidateStack(1, 1);

            if (stack.Peek() is not ParseProductionNode root) throw new InvalidOperationException();
            stack.Pop();

            return root;
        }

        public ProductionBuilder Terminal(string name)
        {
            ValidateTerminalName(name);
            stack.Push(GetTerminal(name));

            return this;
        }

        private ParseTerminalNode GetTerminal(string name)
        {
            throw NotYetImplemented();
        }

        public ProductionBuilder Empty() { stack.Push(ParseTerminalNode.Empty); return this; }

        public ProductionBuilder Production(string name, int count)
        {
            ValidateStack(count);
            ValidateProductionName(name);

            if (count == 0) { Empty(); return Production(name, 1); }

            ParseNode[] children = new ParseNode[count];

            for (int i = count - 1; i >= 0; --i) children[i] = stack.Pop();

            ParseProductionNode production = new(name, new(children));

            stack.Push(production); Add(production); return this;
        }

        public ProductionBuilder Production(string name)
        {
            throw NotYetImplemented();
        }

        public ProductionBuilder Choose()
        {
            throw NotYetImplemented();
        }

        public ProductionBuilder T(string name) => Terminal(name);
        public ProductionBuilder E() => Empty();
        public ProductionBuilder P(string name, int count) => Production(name, count);
        public ProductionBuilder P(string name) => Production(name);
        public ProductionBuilder C() => Choose();

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

        private void Add(ParseProductionNode production)
        {
            productions[production.Name] = production;
        }
    }

    public partial class ParseData
    {
    }

    public class Parser
    {
        public ParseProductionNode Parse()
        {
            throw NotYetImplemented();
        }
    }
}