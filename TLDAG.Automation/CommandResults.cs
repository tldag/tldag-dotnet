using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace TLDAG.Automation
{
    public class CommandResults
    {
        private readonly List<PSObject> objects;
        public IReadOnlyList<PSObject> Objects => objects;

        private readonly List<Exception> exceptions;
        public IReadOnlyList<Exception> Exceptions => exceptions;

        public CommandResults(PowerShell shell, Collection<PSObject> objects)
        {
            this.objects = new(objects);
            exceptions = GetExceptions(shell);
        }

        protected CommandResults(PowerShell shell)
        {
            objects = new();
            exceptions = GetExceptions(shell);
        }

        public virtual CommandResults ThrowExceptions(bool throwOnError)
        {
            if (throwOnError && exceptions.Count > 0) throw exceptions[0];
            return this;
        }

        private static List<Exception> GetExceptions(PowerShell shell)
            => shell.Streams.Error.Select(e => e.Exception ?? new Exception(e.ToString())).ToList();
    }

    public class CommandResults<T> : CommandResults
    {
        private readonly List<T> values;
        public IReadOnlyList<T> Values => values;

        public T Value => Values.First();

        public CommandResults(PowerShell shell, Collection<T> values) : base(shell) { this.values = new(values); }

        public override CommandResults<T> ThrowExceptions(bool throwOnError)
        { base.ThrowExceptions(throwOnError); return this; }
    }
}
