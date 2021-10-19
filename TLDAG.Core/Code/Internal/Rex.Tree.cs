using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Errors;
using static TLDAG.Core.Code.Constants;
using System;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Rex
    {
        internal abstract class Node : Code.Rex.INode
        {
            private bool? nullable = null;
            public bool Nullable => nullable ??= ComputeNullable();

            private UIntSet? firstpos = null;
            public UIntSet Firstpos => firstpos ??= ComputeFirstpos();

            private UIntSet? lastpos = null;
            public UIntSet Lastpos => lastpos ??= ComputeLastpos();

            public virtual V VisitDepthFirst<V>(V visitor) where V : Code.Rex.IVisitor => Visit(visitor);
            public virtual V VisitPreOrder<V>(V visitor) where V : Code.Rex.IVisitor => Visit(visitor);
            protected virtual V Visit<V>(V visitor) where V : Code.Rex.IVisitor { visitor.Visit(this); return visitor; }

            protected abstract bool ComputeNullable();
            protected abstract UIntSet ComputeFirstpos();
            protected abstract UIntSet ComputeLastpos();
        }

        internal abstract class LeafNode : Node
        {
            public readonly uint Id;
            public UIntSet Followpos = UIntSet.Empty;

            protected LeafNode(uint id) { Id = id; }

            protected override UIntSet ComputeFirstpos() => new(Id);
            protected override UIntSet ComputeLastpos() => new(Id);
        }

        internal class AcceptNode : LeafNode
        {
            public readonly string Name;

            public AcceptNode(uint id, string name) : base(id) { Name = name; }

            protected override bool ComputeNullable() => false;
        }

        internal class EmptyNode : LeafNode
        {
            public EmptyNode(uint id) : base(id) { }

            protected override bool ComputeNullable() => true;
        }

        internal class SymbolNode : LeafNode
        {
            public readonly char Value;

            public SymbolNode(uint id, char value) : base(id) { Value = value; }

            protected override bool ComputeNullable() => false;
        }

        internal class NotNode : Node
        {
            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal abstract class BinaryNode : Node
        {
            public Node Left;
            public Node Right;

            public BinaryNode(Node left, Node right) { Left = left; Right = right; }

            public override V VisitDepthFirst<V>(V visitor)
            { Left.VisitDepthFirst(visitor); Right.VisitDepthFirst(visitor); return Visit(visitor); }

            public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Left.VisitPreOrder(visitor); Right.VisitPreOrder(visitor); return visitor; }
        }

        internal class ChooseNode : BinaryNode
        {
            public ChooseNode(Node left, Node right) : base(left, right) { }

            protected override bool ComputeNullable() => Left.Nullable || Right.Nullable;
            protected override UIntSet ComputeFirstpos() => Left.Firstpos + Right.Firstpos;
            protected override UIntSet ComputeLastpos() => Left.Lastpos + Right.Lastpos;
        }

        internal class ConcatNode : BinaryNode
        {
            public ConcatNode(Node left, Node right) : base(left, right) { }

            protected override bool ComputeNullable() => Left.Nullable && Right.Nullable;
            protected override UIntSet ComputeFirstpos() => Left.Nullable ? (Left.Firstpos + Right.Firstpos) : Left.Firstpos;
            protected override UIntSet ComputeLastpos() => Right.Nullable ? (Left.Lastpos + Right.Lastpos) : Right.Lastpos;
        }

        internal class KleeneNode : Node
        {
            public Node Child;

            public KleeneNode(Node child) { Child = child; }

            public override V VisitDepthFirst<V>(V visitor)
            { Child.VisitDepthFirst(visitor); return Visit(visitor); }

            public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Child.VisitPreOrder(visitor); return visitor; }

            protected override bool ComputeNullable() => true;
            protected override UIntSet ComputeFirstpos() => Child.Firstpos;
            protected override UIntSet ComputeLastpos() => Child.Lastpos;
        }

        internal class Builder
        {
            private readonly Stack<Node> stack = new();
            private readonly HashSet<string> names = new(ReservedTokenNames, StringComparer.Ordinal);

            private uint nextId;

            public Builder()
            {
                throw NotYetImplemented();
            }

            public void Accept(string name)
            {
                if (names.Contains(name)) throw new ArgumentException("Duplicate name");
                if (!TokenNameRegex.IsMatch(name)) throw new ArgumentException("Illegal name");

                names.Add(name); stack.Push(new AcceptNode(nextId++, name));
            }

            public void Empty() { stack.Push(new EmptyNode(nextId++)); }
            public void Symbol(char value) { stack.Push(new SymbolNode(nextId++, value)); }
            public void Range(char start, char end) { throw NotYetImplemented(); }
            public void Not(IEnumerable<char> values) { throw NotYetImplemented(); }
            public void Choose() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ChooseNode(left, right)); }
            public void Concat() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ConcatNode(left, right)); }
            public void Kleene() { stack.Push(new KleeneNode(stack.Pop())); }

            public Node Build()
            {
                if (stack.Count > 1) throw new InvalidOperationException();
                if (stack.Count == 0) Empty();

                return stack.Pop();
            }
        }
    }
}