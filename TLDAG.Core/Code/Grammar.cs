using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        public static Scanner CreateScanner(string text) => throw NotYetImplemented();
        public static Parser CreateParser(string text) => throw NotYetImplemented();
        public static ParserCompiler CreateCompiler() => throw NotYetImplemented();

#if DEBUG
        public static Scanner CreateDevScanner(string text) => throw NotYetImplemented();
        public static Parser CreateDevParser(string text) => throw NotYetImplemented();
        public static ParserCompiler CreateDevCompiler() => ParserCompiler.Create(GrammarDocument);
#endif
    }
}
