using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public partial class RexData
    {
        public StringSet Names { get; }

        public RexData(StringSet names) { Names = names; }
    }

    public class RexExpandTree : IRexNodeVisitor
    {
        public void Visit(RexNode node)
        {
            if (node is RexBinaryNode binaryNode) ExpandBinary(binaryNode);
            if (node is RexKleeneNode kleeneNode) ExpandKleene(kleeneNode);
        }

        private static void ExpandBinary(RexBinaryNode binaryNode)
        { binaryNode.Left = InternalExpand(binaryNode.Left); binaryNode.Right = InternalExpand(binaryNode.Right); }

        private static void ExpandKleene(RexKleeneNode kleeneNode)
        { kleeneNode.Child = InternalExpand(kleeneNode.Child); }

        private static RexNode InternalExpand(RexNode node)
        {
            // TODO: Expand RexNotNode

            return node;
        }

        public static RexNode Expand(RexNode root)
        {
            RexExpandTree visitor = new();

            root.VisitPreOrder(visitor);

            return InternalExpand(root);
        }
    }

    public class RexInitTree : IRexNodeVisitor
    {
        private int nextId = 1;

        public void Visit(RexNode node)
        {
            if (node is RexLeafNode leafNode) InitLeaf(leafNode);
            if (node is RexAcceptNode acceptNode) InitAccept(acceptNode);
            if (node is RexEmptyNode emptyNode) InitEmpty(emptyNode);
            if (node is RexSymbolNode symbolNode) InitSymbol(symbolNode);
            if (node is RexChooseNode chooseNode) InitChoose(chooseNode);
            if (node is RexConcatNode concatNode) InitConcat(concatNode);
            if (node is RexKleeneNode kleeneNode) InitKleene(kleeneNode);
        }

        private void InitLeaf(RexLeafNode leafNode) { leafNode.Id = nextId++; }

        private void InitAccept(RexAcceptNode acceptNode)
        {
            throw new NotImplementedException();
        }

        private void InitEmpty(RexEmptyNode emptyNode) { emptyNode.Nullable = true; }

        private void InitSymbol(RexSymbolNode symbolNode)
        {
            throw new NotImplementedException();
        }

        private void InitChoose(RexChooseNode chooseNode)
        {
            throw new NotImplementedException();
        }

        private void InitConcat(RexConcatNode concatNode)
        {
            throw new NotImplementedException();
        }

        private void InitKleene(RexKleeneNode kleeneNode)
        {
            throw new NotImplementedException();
        }

        public static RexInitTree Init(RexNode root)
        {
            RexInitTree init = new();

            root.VisitDepthFirst(init);

            return init;
        }
    }

    public class RexState
    {
    }

    public class RexCompiler
    {
        private readonly RexNode root;
        private readonly SmartMap<IntSet, RexState> states = new();
        private readonly Queue<RexState> unmarked = new();

        public RexCompiler(RexNode root)
        {
            this.root = RexExpandTree.Expand(root);

            RexInitTree init = RexInitTree.Init(root);
        }

        public RexData Compile()
        {
            CreateStates();
            CreateTransitions();
            CreateAccepting();

            throw new NotImplementedException();
        }

        private void CreateStates()
        {
            AddState(IntSet.Empty, false);
            AddState(root.Firstpos);

            while (unmarked.Count > 0) ProcessState(unmarked.Dequeue());
        }

        private void ProcessState(RexState state)
        {
            throw new NotImplementedException();
        }

        private RexState AddState(IntSet positions, bool asUnmarked = true)
        {
            RexState? state = states[positions];

            if (state == null)
            {
                string? accept = GetAccept(positions);

                state = new();
                states[positions] = state;
                if (asUnmarked) unmarked.Enqueue(state);
            }

            return state;
        }

        private string? GetAccept(IntSet positions)
        {
            throw new NotImplementedException();
        }

        private void CreateTransitions()
        {
            throw new NotImplementedException();
        }

        private void CreateAccepting()
        {
            throw new NotImplementedException();
        }

        public static RexCompiler Create(RexNode root) => new(root);
        public static RexData Compile(RexNode root) => Create(root).Compile();
    }
}
