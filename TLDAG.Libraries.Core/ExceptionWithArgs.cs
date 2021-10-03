using System;

namespace TLDAG.Libraries.Core
{
    public class ExceptionWithArgs : Exception
    {
        public ExceptionWithArgs(string format, params object[] args)
            : base(string.Format(format, args)) { }
    }
}
