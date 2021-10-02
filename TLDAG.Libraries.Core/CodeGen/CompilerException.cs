using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class CompilerException : ExceptionWithArgs
    {
        public CompilerException(string format, params object[] args)
            : base(format, args) { }
    }
}
