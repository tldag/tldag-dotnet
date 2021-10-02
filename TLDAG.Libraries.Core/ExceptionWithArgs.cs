using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core
{
    public class ExceptionWithArgs : Exception
    {
        public ExceptionWithArgs(string format, params object[] args)
            : base(string.Format(format, args)) { }
    }
}
