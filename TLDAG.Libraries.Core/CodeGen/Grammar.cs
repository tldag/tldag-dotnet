using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Libraries.Core.CodeGen.Parse;

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
