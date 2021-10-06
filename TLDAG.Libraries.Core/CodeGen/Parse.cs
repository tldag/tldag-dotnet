using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;
using static TLDAG.Libraries.Core.CodeGen.Code;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class ParseNode
    {

    }

    public class ParseTerminal : ParseNode
    {

    }

    public class ParseProduction : ParseNode
    {
        public readonly string Name;

        public ParseProduction(string name) { Name = name; }
    }

    public class ProductionBuilder
    {
        private readonly StringSet terminalNames;
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
            stack.Push(new ParseTerminal());

            return this;
        }

        public ProductionBuilder Production(string name, int count)
        {
            ValidateStack(count);
            ValidateProductionName(name);

            ParseNode[] children = new ParseNode[count];

            for (int i = count - 1; i >= 0; --i) children[i] = stack.Pop();

            ParseProduction production = new(name);

            stack.Push(production); Add(production); return this;
        }

        public ProductionBuilder T(string name) => Terminal(name);
        public ProductionBuilder P(string name, int count) => Production(name, count);

        private void ValidateStack(int min, int max = int.MaxValue)
        {
            if (stack.Count < min) throw new InvalidOperationException();
            if (stack.Count > max) throw new InvalidOperationException();
        }

        private void ValidateTerminalName(string name)
        {
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