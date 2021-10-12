using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Rex
    {
        internal abstract class Visitor : Code.Rex.IVisitor
        {
            public void Visit(Code.Rex.INode node)
            {
                throw NotYetImplemented();
            }

            protected abstract void VisitNode(Node node);
        }

        internal class TransitionsBuilder
        {
            private readonly int width;
            private readonly List<int[]> list = new();

            public TransitionsBuilder(int width) { this.width = width; }
            public static TransitionsBuilder Create(int width) => new(width);

            public TransitionsBuilder Add(int[] transitions)
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

            public static Alphabet Compute(Node root)
            {
                GetAlphabet visitor = new();

                root.VisitDepthFirst(visitor);

                return new(visitor.symbols);
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

        internal class InitTree : Visitor
        {
            private uint nextId = 1;

            protected override void VisitNode(Node node)
            {
                if (node is LeafNode leafNode) InitLeaf(leafNode);
                if (node is AcceptNode acceptNode) InitAccept(acceptNode);
                if (node is EmptyNode emptyNode) InitEmpty(emptyNode);
                if (node is SymbolNode symbolNode) InitSymbol(symbolNode);
                if (node is ChooseNode chooseNode) InitChoose(chooseNode);
                if (node is ConcatNode concatNode) InitConcat(concatNode);
                if (node is KleeneNode kleeneNode) InitKleene(kleeneNode);
                if (node is NotNode) throw new NotSupportedException();
            }

            private void InitLeaf(LeafNode leafNode) { leafNode.Id = nextId++; }

            private void InitAccept(AcceptNode acceptNode)
            {
                throw NotYetImplemented();
            }

            private void InitEmpty(EmptyNode emptyNode) { emptyNode.Nullable = true; }

            private void InitSymbol(SymbolNode symbolNode)
            {
                throw NotYetImplemented();
            }

            private void InitChoose(ChooseNode chooseNode)
            {
                throw NotYetImplemented();
            }

            private void InitConcat(ConcatNode concatNode)
            {
                throw NotYetImplemented();
            }

            private void InitKleene(KleeneNode kleeneNode)
            {
                throw NotYetImplemented();
            }

            public static void Init(Node root) { root.VisitDepthFirst(new InitTree()); }
        }

        internal class State
        {
            public readonly int Id;
            public readonly string? Accept;
            private readonly int[] transitions;

            public int[] Transitions => Arrays.Copy(transitions);

            public State(int id, string? accept, int width)
            {
                Id = id;
                Accept = accept;
                transitions = new int[width];
            }
        }

        internal class Compiler
        {
            private readonly Alphabet alphabet;
            private readonly int width;

            private readonly Node root;

            private readonly SmartMap<UIntSet, State> states = new();
            private readonly Queue<State> unmarked = new();

            public Compiler(Code.Rex.INode input)
            {
                root = input as Node ?? throw new NotSupportedException();

                alphabet = GetAlphabet.Compute(root);
                width = alphabet.Count;

                root = ExpandTree.Expand(root, alphabet);

                InitTree.Init(root);
            }

            public static Compiler Create(Code.Rex.INode root) => new(root);
            public static Code.Rex.IData Compile(Code.Rex.INode root) => Create(root).Compile();

            public Code.Rex.IData Compile()
            {
                CreateStates();

                Transitions transitions = CreateTransitions();
                Accepts accepts = CreateAccepts();

                return new Data(accepts, 1);
            }

            private void CreateStates()
            {
                AddState(UIntSet.Empty, false);
                AddState(root.Firstpos);

                while (unmarked.Count > 0) ProcessState(unmarked.Dequeue());
            }

            private void ProcessState(State state)
            {
                throw NotYetImplemented();
            }

            private State AddState(UIntSet positions, bool asUnmarked = true)
            {
                State? state = states[positions];

                if (state == null)
                {
                    string? accept = GetAccept(positions);

                    state = new(states.Count, accept, width);
                    states[positions] = state;
                    if (asUnmarked) unmarked.Enqueue(state);
                }

                return state;
            }

            private string? GetAccept(UIntSet positions)
            {
                if (positions.Count == 0) return "";

                foreach (int position in positions)
                {
                    throw NotYetImplemented();
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
                IntMap<string> accepts = new();

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