namespace TLDAG.Libraries.Core.CodeGen
{
    public class CompilerException : ExceptionWithArgs
    {
        public CompilerException(string format, params object[] args)
            : base(format, args) { }
    }
}
