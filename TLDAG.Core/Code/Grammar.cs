using System;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        public static Scanner CreateScanner(string text) => throw NotYetImplemented();
        public static Parser CreateParser(string text) => throw NotYetImplemented();
        public static ParseCompiler CreateCompiler() => throw NotYetImplemented();

#if DEBUG
        public static Scanner CreateDevScanner(string text) => new();
        public static Parser CreateDevParser(string text) => throw NotYetImplemented();
        public static ParseCompiler CreateDevCompiler() => ParseCompiler.Create(GrammarDocument);
#endif
    }
}
