using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class ParseNode
    {

    }

    public class Terminal : ParseNode
    {

    }

    public class Production : ParseNode
    {

    }

    public class ProductionBuilder
    {
        private StringSet terminalNames => throw new InvalidOperationException();

        private readonly Stack<ParseNode> stack = new();

        public ProductionBuilder(RexData rex) { }

        public static ProductionBuilder Create(RexData rex) => new(rex);

        public Production Build()
        {
            if (stack.Count != 1) throw new InvalidOperationException();
            if (stack.Peek() is not Production root) throw new InvalidOperationException();

            stack.Pop();

            return root;
        }

        public ProductionBuilder Terminal(string name)
        {
            if (!terminalNames.Contains(name)) throw new ArgumentException();

            stack.Push(new Terminal());

            return this;
        }

        public ProductionBuilder NonTerminal(string name, int count)
        {
            throw new NotImplementedException();
        }

        public ProductionBuilder T(string name) => Terminal(name);
        public ProductionBuilder P(string name, int count) => NonTerminal(name, count);
    }

    public class Parser
    {
        public Production Parse()
        {
            throw new NotImplementedException();
        }
    }
}