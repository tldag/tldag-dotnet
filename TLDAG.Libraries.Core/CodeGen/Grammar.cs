using System;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        public static Scanner CreateScanner(string text) => throw new NotImplementedException();
        public static Parser CreateParser(string text) => throw new NotImplementedException();
        public static ParseCompiler CreateCompiler() => throw new NotImplementedException();

#if DEBUG
        public static Scanner CreateDevScanner(string text) => new(GrammarRexData, text);
        public static Parser CreateDevParser(string text) => throw new NotImplementedException();
        public static ParseCompiler CreateDevCompiler() => throw new NotImplementedException();
#endif
    }
}
