namespace TLDAG.Core.Code.Internal
{
    internal static partial class Parse
    {
        internal partial class Compiler
        {
            internal class InitTerminals : Visitor
            {
                private uint nextId = Terminal.NextId;
                private readonly TerminalMap terminals = new();

                public override void Visit(Code.Parse.INode node)
                {
                    if (node is Terminal terminal)
                    {
                        if (terminal.Id == 0) { terminal.Id = nextId++; }
                        terminals[terminal.Id] = terminal;
                    }
                }

                public static TerminalDict Init(Node root)
                {
                    InitTerminals visitor = new();
                    root.VisitDepthFirst(visitor);

                    visitor.terminals[Terminal.EndOfFile.Id] = Terminal.EndOfFile;
                    visitor.terminals[Terminal.Empty.Id] = Terminal.Empty;

                    return new(visitor.terminals);
                }
            }

            internal class InitProductions : Visitor
            {
                private readonly ProductionMap productions = new();
                private uint nextId = 1;

                public override void Visit(Code.Parse.INode node)
                {
                    if (node is Production production)
                    {
                        if (production.Id == 0) production.Id = nextId++;
                        productions[production.Id] = production;
                    }
                }

                public static ProductionDict Init(Node root)
                {
                    InitProductions visitor = new();
                    root.VisitPreOrder(visitor);
                    return new(visitor.productions);
                }
            }
        }
    }
}