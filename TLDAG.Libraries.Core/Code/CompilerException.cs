namespace TLDAG.Libraries.Core.Code
{
    public class CompilerException : ExceptionWithArgs
    {
        public CompilerException(string format, params object[] args)
            : base(format, args) { }
    }
}
