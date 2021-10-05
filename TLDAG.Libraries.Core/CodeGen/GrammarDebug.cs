#if DEBUG

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        public partial class Compiler
        {
            public Compiler(ParseProduction root)
            {

            }
        }
    }

    public partial class GrammarCompiler
    {
        public GrammarCompiler(Grammar.Compiler compiler)
        {
            this.compiler = compiler;
        }
    }
}

#endif