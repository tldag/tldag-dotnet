#if DEBUG

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        private static readonly ProductionBuilder pbuilder = ProductionBuilder.Create(GrammarRexNames);

        public static readonly ParseProductionNode GrammarDocument
            = pbuilder.P("grammar", 0).Build();
    }
}

#endif