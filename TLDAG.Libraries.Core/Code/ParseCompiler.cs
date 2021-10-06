using System;
using static TLDAG.Libraries.Core.Code.Constants;

namespace TLDAG.Libraries.Core.Code
{
    public partial class ParseData
    {
    }

    public class ParseCompiler
    {
        private ParseNode root;

        public ParseCompiler(ParseNode root)
        {
            this.root = Extend(root);
        }

        private static ParseProduction Extend(ParseNode root)
        {
            ParseNode[] children = { root, ParseTerminal.EndOfFile };

            return new(ExtendedGrammarRootName, children);
        }

        public static ParseCompiler Create(ParseNode root) => new(root);

        public ParseData Compile()
        {
            throw new NotImplementedException();
        }
    }
}
