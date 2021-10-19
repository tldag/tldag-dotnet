using System.Collections.Generic;
using System.IO;
using static TLDAG.Core.Errors;

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
            public uint Id { get; }

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
        private readonly Internal.Parse.Builder builder;

        public ParserBuilder(IEnumerable<string> tokenNames)
        {
            builder = new(tokenNames);
        }

        public ParserBuilder Production(string name, int count) { builder.Production(name, count); return this; }

        public ParserBuilder P(string name, int count) => Production(name, count);

        public Parse.IProduction Build() => builder.Build();
    }

    public class ParserCompiler
    {
        private readonly Internal.Parse.Compiler compiler;

        public ParserCompiler(Parse.INode root)
        {
            compiler = new(root);
        }

        public static ParserCompiler Create(Parse.INode root) => new(root);

        public Parse.IData Compile() => compiler.Compile();
    }

    public class Parser
    {
    }
}