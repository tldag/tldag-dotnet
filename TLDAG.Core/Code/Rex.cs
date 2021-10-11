using System.Collections.Generic;
using System.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public static class Rex
    {
        public interface INode
        {
            public V VisitDepthFirst<V>(V visitor) where V : IVisitor;
            public V VisitPreOrder<V>(V visitor) where V : IVisitor;
        }

        public static INode Empty() => new Internal.Rex.EmptyNode();

        public interface IVisitor
        {
            public void Visit(INode node);
        }

        public interface IData
        {
            void Save(Stream stream);
            byte[] Save(bool compress);
        }

        public static IData Load(Stream stream) => throw NotYetImplemented();
        public static IData Load(byte[] bytes, bool compressed) => throw NotYetImplemented();
    }

    public class RexBuilder
    {
        private Internal.Rex.Builder builder = new();

        public RexBuilder Accept(string name) { builder.Accept(name); return this; }
        public RexBuilder Empty() { builder.Empty(); return this; }
        public RexBuilder Symbol(char value) { builder.Symbol(value); return this; }
        public RexBuilder Range(char start, char end) { builder.Range(start, end); return this; }
        public RexBuilder Not(IEnumerable<char> values) { builder.Not(values); return this; }
        public RexBuilder Choose() { builder.Choose(); return this; }
        public RexBuilder Concat() { builder.Concat(); return this; }
        public RexBuilder Kleene() { builder.Kleene(); return this; }

        public Rex.INode Build() { return builder.Build(); }

        public RexBuilder A(string name) => Accept(name);
        public RexBuilder E() => Empty();
        public RexBuilder S(char value) => Symbol(value);
        public RexBuilder R(char start, char end) => Range(start, end);
        public RexBuilder N(IEnumerable<char> values) => Not(values);
        public RexBuilder CH() => Choose();
        public RexBuilder CN() => Concat();
        public RexBuilder K() => Kleene();
    }
}
