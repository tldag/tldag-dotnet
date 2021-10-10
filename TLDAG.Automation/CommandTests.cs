using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Automation
{
    public abstract class CommandTests
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
