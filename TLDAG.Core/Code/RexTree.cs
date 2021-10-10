using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public partial class Rex
    {
        internal interface IVisitor { void Visit(Node node); }

        internal abstract class Node : INode
        {
            public bool Nullable = false;
            public IntSet Firstpos = IntSet.Empty;
            public IntSet Lastpos = IntSet.Empty;

            public virtual V VisitDepthFirst<V>(V visitor) where V : IVisitor => Visit(visitor);
            public virtual V VisitPreOrder<V>(V visitor) where V : IVisitor => Visit(visitor);
            protected virtual V Visit<V>(V visitor) where V : IVisitor { visitor.Visit(this); return visitor; }

            public abstract Node Clone();
        }

        internal abstract class LeafNode : Node
        {
            public int Id = 0;
            public IntSet Followpos = IntSet.Empty;
        }

        internal class AcceptNode : LeafNode
        {
            public readonly string Name;

            public AcceptNode(string name) { Name = name; }

            public override Node Clone() { throw new NotSupportedException(); }
        }

        internal class EmptyNode : LeafNode
        {
            public override Node Clone() => new EmptyNode();
        }

        internal class SymbolNode : LeafNode
        {
            public readonly char Value;

            public SymbolNode(char value) { Value = value; }

            public override Node Clone() => new SymbolNode(Value);
        }

        internal class NotNode : Node
        {
            public override Node Clone() => throw NotYetImplemented();
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

            public override Node Clone() => new ChooseNode(Left.Clone(), Right.Clone());
        }

        internal class ConcatNode : BinaryNode
        {
            public ConcatNode(Node left, Node right) : base(left, right) { }

            public override Node Clone() => new ConcatNode(Left.Clone(), Right.Clone());
        }

        internal class KleeneNode : Node
        {
            public Node Child;

            public KleeneNode(Node child) { Child = child; }

            public override V VisitDepthFirst<V>(V visitor)
            { Child.VisitDepthFirst(visitor); return Visit(visitor); }

            public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Child.VisitPreOrder(visitor); return visitor; }

            public override Node Clone() => new KleeneNode(Child.Clone());
        }

        internal class Builder
        {
            private readonly Stack<Node> stack = new();
            private readonly HashSet<string> names = new(ReservedTokenNames, StringComparer.Ordinal);

            public void Accept(string name)
            {
                if (names.Contains(name)) throw new ArgumentException("Duplicate name");
                if (!TokenNameRegex.IsMatch(name)) throw new ArgumentException("Illegal name");

                names.Add(name); stack.Push(new AcceptNode(name));
            }

            public void Empty() { stack.Push(new EmptyNode()); }
            public void Symbol(char value) { stack.Push(new SymbolNode(value)); }
            public void Range(char start, char end) { throw NotYetImplemented(); }
            public void Not(IEnumerable<char> values) { throw NotYetImplemented(); }
            public void Choose() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ChooseNode(left, right)); }
            public void Concat() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ConcatNode(left, right)); }
            public void Kleene() { stack.Push(new KleeneNode(stack.Pop())); }

            public Node Build()
            {
                if (stack.Count == 0) return new EmptyNode();
                if (stack.Count > 1) throw new InvalidOperationException();

                return stack.Pop();
            }
        }
    }
}
