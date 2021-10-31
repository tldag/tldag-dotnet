using System.IO;
using static TLDAG.Core.Executing.Java.JavaExecutable;

namespace TLDAG.Core.Executing.Java
{
    public static class Java
    {
        public static bool HasJava() => TryFindJava(out Executable _);

        public static ExecutionResult ExecuteJava(FileInfo jarFile, params string[] args)
        {
            return JavaExecutionBuilder.Create(FindJava(), jarFile)
                .AddArguments(args)
                .Build().Execute();
        }
    }
}
