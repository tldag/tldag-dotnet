using System;
using TLDAG.Automation;

namespace TLDAG.Test
{
    public abstract class CommandTests : TestsBase
    {
        protected abstract Type CommandType { get; }

        private Shell? shell;
        protected Shell Shell => shell ??= new(CommandType);

        protected T Invoke<T>(string script) => GetResults<T>(script).Value;
        protected void Invoke(string script) { GetResults(script); }

        protected CommandResults<T> GetResults<T>(string script, bool throwOnError = true)
            => Shell.Invoke<T>(script).ThrowExceptions(throwOnError);

        protected CommandResults GetResults(string script, bool throwOnError = true)
            => Shell.Invoke(script).ThrowExceptions(throwOnError);
    }
}
