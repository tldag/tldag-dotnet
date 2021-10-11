using System.Collections.Generic;
using System.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public static class Parse
    {
        public interface IVisitor
        {
            public void Visit(INode node);
        }

        public interface INode
        {
            public int Id { get; }

            public V VisitDepthFirst<V>(V visitor) where V : IVisitor;
            public V VisitPreOrder<V>(V visitor) where V : IVisitor;
        }

        public interface ITerminal : INode
        {
            public string Name { get; }
        }

        public interface IProduction : INode
        {
            public string Name { get; }
            public IEnumerable<INode> Children { get; }
            public int Count { get; }
        }

        public interface IData
        {
            void Save(Stream stream);
            byte[] Save(bool compress);
        }

        public static IData Load(Stream stream) => throw NotYetImplemented();
        public static IData Load(byte[] bytes, bool compressed) => throw NotYetImplemented();
    }

    public class ParserBuilder
    {
        public ParserBuilder(IEnumerable<string> tokenNames)
        {
            throw NotYetImplemented();
        }

        public ParserBuilder P(string name, int count) => throw NotYetImplemented();

        public Parse.IProduction Build() => throw NotYetImplemented();
    }

    public class ParserCompiler
    {
        public static ParserCompiler Create(Parse.INode root) => throw NotYetImplemented();

        public Parse.IData Compile() => throw NotYetImplemented();
    }

    public class Parser
    {
    }
}