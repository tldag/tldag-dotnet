#if NET5_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

#endif