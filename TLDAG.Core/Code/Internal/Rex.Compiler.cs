using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using TLDAG.Core.Collections;
using static TLDAG.Core.Errors;
using static TLDAG.Core.Code.Rex;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Rex
    {
        public class Leafs : UIntMap<LeafNode> { }

        internal abstract class Visitor : IVisitor
        {
            public void Visit(INode node)
            {
                throw NotYetImplemented();
            }

            protected abstract void VisitNode(Node node);
        }

        internal class TransitionsBuilder
        {
            private readonly int width;
            private readonly List<uint[]> list = new();

            public TransitionsBuilder(int width) { this.width = width; }
            public static TransitionsBuilder Create(int width) => new(width);

            public TransitionsBuilder Add(uint[] transitions)
            {
                if (transitions.Length != width) throw new ArgumentException();
                list.Add(transitions); return this;
            }

            public Transitions Build() => new(width, list.ToArray());
        }

        internal class GetAlphabet : Visitor
        {
            private readonly List<char> symbols = new();

            protected override void VisitNode(Node node)
            {
                if (node is SymbolNode symbolNode) symbols.Add(symbolNode.Value);
                else if (node is NotNode notNode) throw NotYetImplemented();
            }

            public static Alphabet Collect(Node root)
            {
                GetAlphabet visitor = new();

                root.VisitDepthFirst(visitor);

                return new(visitor.symbols);
            }
        }

        internal class GetLeafs
        {
            public static Leafs Collect(Node root)
            {
                throw NotYetImplemented();
            }
        }

        internal class GetAccepts
        {
            public static UIntMap<string> Collect(Node root)
            {
                throw NotYetImplemented();
            }
        }

        internal class ExpandTree : Visitor
        {
            protected override void VisitNode(Node node)
            {
                if (node is BinaryNode binaryNode) ExpandBinary(binaryNode);
                else if (node is KleeneNode kleeneNode) ExpandKleene(kleeneNode);
            }

            private static void ExpandBinary(BinaryNode binaryNode)
            { binaryNode.Left = InternalExpand(binaryNode.Left); binaryNode.Right = InternalExpand(binaryNode.Right); }

            private static void ExpandKleene(KleeneNode kleeneNode)
            { kleeneNode.Child = InternalExpand(kleeneNode.Child); }

            private static Node InternalExpand(Node node)
            {
                if (node is NotNode notNode) return ExpandNotNode(notNode);

                return node;
            }

            private static Node ExpandNotNode(NotNode notNode)
            {
                throw NotYetImplemented();
            }

            public static Node Expand(Node root, Alphabet alphabet)
            {
                ExpandTree visitor = new();

                root.VisitPreOrder(visitor);

                return InternalExpand(root);
            }
        }

        internal class State
        {
            public readonly uint Id;
            public readonly UIntSet Positions;
            public readonly string? Accept;
            private readonly uint[] transitions;

            public uint[] Transitions => Arrays.Copy(transitions);

            public State(uint id, UIntSet positions, string? accept, int width)
            {
                Id = id;
                Positions = positions;
                Accept = accept;
                transitions = new uint[width];
            }

            public void SetTransition(uint symbol, uint state)
            {
                throw NotYetImplemented();
            }
        }

        internal class States : SmartMap<UIntSet, State>
        {
            public uint NextId => throw NotYetImplemented();
        }

        internal class Compiler
        {
            private readonly Alphabet alphabet;
            private readonly int width;

            private readonly Node root;
            private readonly Leafs leafs;
            private readonly UIntMap<string> accepts;

            private readonly States states = new();
            private readonly Queue<State> unmarked = new();

            public Compiler(INode input)
            {
                root = input as Node ?? throw new NotSupportedException();

                alphabet = GetAlphabet.Collect(root);
                width = alphabet.Count;

                root = ExpandTree.Expand(root, alphabet);
                leafs = GetLeafs.Collect(root);
                accepts = GetAccepts.Collect(root);
            }

            public static Compiler Create(INode root) => new(root);
            public static IData Compile(INode root) => Create(root).Compile();

            public Data Compile()
            {
                CreateStates();

                Transitions transitions = CreateTransitions();
                Accepts accepts = CreateAccepts();

                return new Data(alphabet, transitions, accepts, 1);
            }

            private void CreateStates()
            {
                AddState(UIntSet.Empty, false);
                AddState(root.Firstpos);

               while (unmarked.Count > 0) { ProcessState(unmarked.Dequeue()); }
            }

            private void ProcessState(State state)
                { foreach (uint symbol in alphabet) { ProcessStateSymbol(state, symbol); } }

            private void ProcessStateSymbol(State state, uint symbol)
            {
                if (symbol == 0) return;

                UIntSet positions = UIntSet.Empty;

                foreach (uint position in state.Positions)
                {
                    if (leafs[position] is SymbolNode symbolNode)
                    {
                        positions += alphabet[symbolNode.Value];
                    }
                }

                state.SetTransition(symbol, AddState(positions, true).Id);
            }

            private State AddState(UIntSet positions, bool asUnmarked = true)
            {
                State? state = states[positions];

                if (state == null)
                {
                    string? accept = GetAccept(positions);

                    state = new(states.NextId, positions, accept, width);
                    states[positions] = state;
                    if (asUnmarked) unmarked.Enqueue(state);
                }

                return state;
            }

            private string? GetAccept(UIntSet positions)
            {
                if (positions.Count == 0) return "";

                foreach (uint position in positions)
                {
                    string? accept = accepts[position];
                    if (accept is not null) return accepts[position];
                }

                return null;
            }

            private Transitions CreateTransitions()
            {
                State[] ordered = states.Values.OrderBy(state => state.Id).ToArray();
                int count = ordered.Length;
                TransitionsBuilder builder = new(width);

                for (int i = 0; i < count; ++i) builder.Add(ordered[i].Transitions);

                return builder.Build();
            }

            private Accepts CreateAccepts()
            {
                UIntMap<string> accepts = new();

                foreach (State state in states.Values)
                {
                    string? accept = state.Accept;

                    if (accept is not null) accepts[state.Id] = accept;
                }

                return new(accepts);
            }

        }
    }
}