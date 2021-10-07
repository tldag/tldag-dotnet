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

        protected CommandResults<T> GetResults<T>(string script, bool throwOnError = true)
        {
            CommandResults<T> results = Shell.Invoke<T>(script);

            if (throwOnError) results.ThrowExceptions();

            return results;
        }
    }
}
