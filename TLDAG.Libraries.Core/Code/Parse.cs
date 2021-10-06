using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;
using static TLDAG.Libraries.Core.Code.Constants;

namespace TLDAG.Libraries.Core.Code
{
    public interface IParseNodeVisitor
    {
        public void Visit(ParseNode node);
    }

    public class ParseNode
    {
        public virtual V VisitDepthFirst<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        public virtual V VisitPreOrder<V>(V visitor) where V : IParseNodeVisitor => Visit(visitor);
        protected virtual V Visit<V>(V visitor) where V : IParseNodeVisitor { visitor.Visit(this); return visitor; }
    }

    public class ParseTerminal : ParseNode
    {
        public static readonly ParseTerminal EOF = new();
    }

    public class ParseEmptyNode : ParseNode
    {
        private ParseEmptyNode() { }

        public static readonly ParseEmptyNode Instance = new();
    }

    public class ParseProduction : ParseNode
    {
        public readonly string Name;
        private readonly ParseNode[] children;

        public ParseProduction(string name, ParseNode[] children) { Name = name; this.children = children; }

        public override V VisitDepthFirst<V>(V visitor)
        {
            foreach (ParseNode node in children) node.VisitDepthFirst(visitor);
            return base.Visit(visitor);
        }
    }

    public class ProductionBuilder
    {
        private readonly StringSet terminalNames;
        private readonly SmartMap<string, ParseTerminal> terminals = new();
        private readonly SmartMap<string, ParseProduction> productions = new();

        private readonly Stack<ParseNode> stack = new();

        public ProductionBuilder(RexData rex)
        {
            terminalNames = rex.Names;
        }

        public static ProductionBuilder Create(RexData rex) => new(rex);

        public ParseProduction Build()
        {
            ValidateStack(1, 1);

            if (stack.Peek() is not ParseProduction root) throw new InvalidOperationException();
            stack.Pop();

            return root;
        }

        public ProductionBuilder Terminal(string name)
        {
            ValidateTerminalName(name);
            stack.Push(GetTerminal(name));

            return this;
        }

        private ParseTerminal GetTerminal(string name)
        {
            throw new NotImplementedException();
        }

        public ProductionBuilder Empty() { stack.Push(ParseEmptyNode.Instance); return this; }

        public ProductionBuilder Production(string name, int count)
        {
            ValidateStack(count);
            ValidateProductionName(name);

            ParseNode[] children = new ParseNode[count];

            for (int i = count - 1; i >= 0; --i) children[i] = stack.Pop();

            ParseProduction production = new(name, children);

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

        private void Add(ParseProduction production)
        {
            productions[production.Name] = production;
        }
    }

    public class Parser
    {
        public ParseProduction Parse()
        {
            throw new NotImplementedException();
        }
    }
}