using System;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public partial class ParseData
    {
    }

    public class ParseInitTerminals : IParseNodeVisitor
    {
        private readonly IntMap<ParseTerminal> terminals = new();

        public void Visit(ParseNode node)
        {
            if (node is ParseTerminal terminal)
            {
                if (terminal.Id == 0)
                {
                    terminal.Id = terminals.Count + 1;
                    terminals[terminal.Id] = terminal;
                }
            }
        }

        public static IntMap<ParseTerminal> Init(ParseNode root)
        {
            ParseInitTerminals visitor = new();

            root.VisitDepthFirst(visitor);

            return visitor.terminals;
        }
    }

    public class ParseCompiler
    {
        private readonly ParseNode root;
        private readonly IntMap<ParseTerminal> terminals;

        public ParseCompiler(ParseNode root)
        {
            this.root = Extend(root);

            terminals = ParseInitTerminals.Init(this.root);
        }

        private static ParseProduction Extend(ParseNode root)
        {
            ParseNode[] children = { root, ParseTerminal.EndOfFile };

            return new(ExtendedGrammarRootName, children);
        }

        public static ParseCompiler Create(ParseNode root) => new(root);

        public ParseData Compile()
        {
            throw new NotImplementedException();
        }
    }
}
