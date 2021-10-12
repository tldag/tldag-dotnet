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
        internal partial class Compiler
        {
            private readonly Production root;
            private readonly TerminalDict terminals;
            private readonly ProductionDict productions;

            private readonly UIntSet terminalIds;

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
                Node root = Contract.As<Node>(node, nameof(node));
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
                            foreach (Nodes follow in element.Follow(production, element.Terminal))
                            {
                                foreach (uint terminalId in GetFirst(follow))
                                {
                                    Terminal terminal = terminals[terminalId] ?? throw NotSupported();
                                    Element candidate = new(production, 0, terminal);

                                    if (collector.Add(candidate)) modified = true;
                                }
                            }
                        }
                    }

                }
                while (modified);

                return collector.Build();
            }

            private UIntSet GetFirst(Nodes nodes) => firsts[nodes] ??= ComputeFirst(nodes);

            private UIntSet ComputeFirst(Nodes nodes) { throw NotYetImplemented(); }

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