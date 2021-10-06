#if DEBUG

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        private static readonly ProductionBuilder pbuilder = ProductionBuilder.Create(GrammarRexData);

        public static readonly ParseProduction GrammarDocument
            = pbuilder.T("EOF").P("grammar", 1).Build();
    }
}

#endif