using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public interface IRexNodeVisitor
    {
        public void Visit(RexNode node);
    }

    public abstract class RexNode
    {
        public bool Nullable = false;
        public IntSet Firstpos = IntSet.Empty;
        public IntSet Lastpos = IntSet.Empty;

        public virtual V VisitDepthFirst<V>(V visitor) where V : IRexNodeVisitor => Visit(visitor);
        public virtual V VisitPreOrder<V>(V visitor) where V : IRexNodeVisitor  => Visit(visitor);
        protected virtual V Visit<V>(V visitor) where V : IRexNodeVisitor { visitor.Visit(this); return visitor; }

        public abstract RexNode Clone();
    }

    public abstract class RexLeafNode : RexNode
    {
        public int Id = 0;
        public IntSet Followpos = IntSet.Empty;
    }

    public class RexAcceptNode : RexLeafNode
    {
        public readonly string Name;

        public RexAcceptNode(string name) { Name = name; }

        public override RexNode Clone() { throw new NotSupportedException(); }
    }

    public class RexEmptyNode : RexLeafNode
    {
        public override RexNode Clone() => new RexEmptyNode();
    }

    public class RexSymbolNode : RexLeafNode
    {
        public readonly char Value;

        public RexSymbolNode(char value) { Value = value; }

        public override RexNode Clone() => new RexSymbolNode(Value);
    }

    public class RexNotNode : RexNode
    {
        public override RexNode Clone() => throw new NotImplementedException();
    }

    public abstract class RexBinaryNode : RexNode
    {
        public RexNode Left;
        public RexNode Right;

        public RexBinaryNode(RexNode left, RexNode right) { Left = left; Right = right; }

        public override V VisitDepthFirst<V>(V visitor)
            { Left.VisitDepthFirst(visitor); Right.VisitDepthFirst(visitor); return Visit(visitor); }

        public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Left.VisitPreOrder(visitor); Right.VisitPreOrder(visitor); return visitor; }
    }

    public class RexChooseNode : RexBinaryNode
    {
        public RexChooseNode(RexNode left, RexNode right) : base(left, right) { }

        public override RexNode Clone() => new RexChooseNode(Left.Clone(), Right.Clone());
    }

    public class RexConcatNode : RexBinaryNode
    {
        public RexConcatNode(RexNode left, RexNode right) : base(left, right) { }

        public override RexNode Clone() => new RexConcatNode(Left.Clone(), Right.Clone());
    }

    public class RexKleeneNode : RexNode
    {
        public RexNode Child;

        public RexKleeneNode(RexNode child) { Child = child; }

        public override V VisitDepthFirst<V>(V visitor)
            { Child.VisitDepthFirst(visitor); return Visit(visitor); }

        public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Child.VisitPreOrder(visitor); return visitor; }

        public override RexNode Clone() => new RexKleeneNode(Child.Clone());
    }

    public class RexBuilder
    {
        private readonly Stack<RexNode> stack = new();
        private readonly HashSet<string> names = new(ReservedTokenNames, StringComparer.Ordinal);

        public RexBuilder Accept(string name)
        {
            if (names.Contains(name)) throw new ArgumentException("Duplicate name");
            if (!TokenNameRegex.IsMatch(name)) throw new ArgumentException("Illegal name");

            names.Add(name); stack.Push(new RexAcceptNode(name)); return this;
        }

        public RexBuilder Empty() { stack.Push(new RexEmptyNode()); return this; }
        public RexBuilder Symbol(char value) { stack.Push(new RexSymbolNode(value)); return this; }
        public RexBuilder Not(IEnumerable<char> values) { throw new NotImplementedException(); }

        public RexBuilder Choose()
        { RexNode right = stack.Pop(); RexNode left = stack.Pop(); stack.Push(new RexChooseNode(left, right)); return this; }

        public RexBuilder Concat()
        { RexNode right = stack.Pop(); RexNode left = stack.Pop(); stack.Push(new RexConcatNode(left, right)); return this; }

        public RexBuilder Kleene() { stack.Push(new RexKleeneNode(stack.Pop())); return this; }

        public RexNode Build()
        {
            if (stack.Count == 0) return new RexEmptyNode();
            if (stack.Count > 1) throw new InvalidOperationException();

            return stack.Pop();
        }

        public RexBuilder A(string name) => Accept(name);
        public RexBuilder E() => Empty();
        public RexBuilder S(char value) => Symbol(value);
        public RexBuilder N(IEnumerable<char> values) => Not(values);
        public RexBuilder CH() => Choose();
        public RexBuilder CN() => Concat();
        public RexBuilder K() => Kleene();
    }

    public partial class RexTransitions
    {
        private readonly int width;
        private readonly int[][] transitions;

        internal RexTransitions(int width, int[][] transitions) { this.width = width; this.transitions = transitions; }
    }

    public partial class RexAccepts
    {
        private readonly IntMap<string> map;

        public StringSet Values => new(map.Values);

        public RexAccepts(IntMap<string> map) { this.map = map; }

        public string? this[int state] { get => map[state]; }
    }

    public partial class RexData
    {
        public readonly RexAccepts Accepts;
        public readonly int StartState;

        public StringSet Names => Accepts.Values;

        public RexData(RexAccepts accepts, int startState)
        {
            Accepts = accepts;
            StartState = startState;
        }

        protected RexData(RexData rex)
        {
            Accepts = rex.Accepts;
            StartState = rex.StartState;
        }
    }
}
