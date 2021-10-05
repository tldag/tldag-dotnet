namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        public partial class Scanner
        {

        }

        public partial class Parser
        {

        }

        public partial class Compiler
        {
            public ParseData Compile(string text)
            {
                return ParseCompiler.Create().Compile();
            }
        }
    }
}
