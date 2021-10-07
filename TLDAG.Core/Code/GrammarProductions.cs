#if DEBUG

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        private static readonly ProductionBuilder pbuilder = ProductionBuilder.Create(GrammarRexData);

        public static readonly ParseProduction GrammarDocument
            = pbuilder.E().P("grammar", 1).Build();
    }
}

#endif