using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public interface IParseNodeVisitor
    {
        public void Visit(ParseNode node);
    }

    public class ParseNode
    {
        private IntSet first = IntSet.Empty;
        public IntSet First => first;

        public bool AddToFirst(int id) { if (first.Contains(id)) return false; first += id; return true; }
        public bool AddToFirst(IntSet ids) { if (first.ContainsAll(ids)) return false; first += ids; return true; }

        public virtual V VisitDepthFirst<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        public virtual V VisitPreOrder<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        protected virtual V Visit<V>(V visitor) where V : IParseNodeVisitor { visitor.Visit(this); return visitor; }
    }

    public class ParseTerminalNode : ParseNode
    {
        public int Id;
        public readonly string Name;

        public ParseTerminalNode(string name) : this(0, name) { }
        private ParseTerminalNode(int id, string name) { Id = id; Name = name; }

        public const int EndOfFileId = 1;
        public const int EmptyId = 2;
        public const int NextId = 3;

        public static readonly ParseTerminalNode EndOfFile = new(EndOfFileId, EndOfFileName);
        public static readonly ParseTerminalNode Empty = new(EmptyId, EmptyNodeName);
    }

    public class ParseProductionNode : ParseNode
    {
        public readonly string Name;
        private readonly ParseNode[] children;
        public IReadOnlyList<ParseNode> Children => children;
        public int Count => children.Length;

        public ParseProductionNode(string name, ParseNode[] children) { Name = name; this.children = children; }

        public override V VisitDepthFirst<V>(V visitor)
        {
            foreach (ParseNode node in children) node.VisitDepthFirst(visitor);
            return base.Visit(visitor);
        }
    }

    public class ProductionBuilder
    {
        private readonly StringSet terminalNames;
        private readonly SmartMap<string, ParseTerminalNode> terminals = new();
        private readonly SmartMap<string, ParseProductionNode> productions = new();

        private readonly Stack<ParseNode> stack = new();

        public ProductionBuilder(RexData rex)
        {
            terminalNames = rex.Names;
        }

        public static ProductionBuilder Create(RexData rex) => new(rex);

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
            throw new NotImplementedException();
        }

        public ProductionBuilder Empty() { stack.Push(ParseTerminalNode.Empty); return this; }

        public ProductionBuilder Production(string name, int count)
        {
            ValidateStack(count);
            ValidateProductionName(name);

            if (count == 0) { Empty(); return Production(name, 1); }

            ParseNode[] children = new ParseNode[count];

            for (int i = count - 1; i >= 0; --i) children[i] = stack.Pop();

            ParseProductionNode production = new(name, children);

            stack.Push(production); Add(production); return this;
        }

        public ProductionBuilder Production(string name)
        {
            throw new NotImplementedException();
        }

        public ProductionBuilder T(string name) => Terminal(name);
        public ProductionBuilder E() => Empty();
        public ProductionBuilder P(string name, int count) => Production(name, count);
        public ProductionBuilder P(string name) => Production(name);

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
            throw new NotImplementedException();
        }
    }
}