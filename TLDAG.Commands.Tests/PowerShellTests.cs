using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Commands.Tests
{
    public abstract class PowerShellTests
    {
        private PowerShell? shell = null;
        protected PowerShell Shell => shell ??= CreateShell();

        protected virtual PowerShell CreateShell()
        {
            PowerShell shell = PowerShell.Create();
            string assemblyPath = Command.Assembly.Location;

            shell.AddScript($"Import-Module '{assemblyPath}'");
            shell.Invoke();

            Assert.IsFalse(shell.HadErrors);
            shell.Commands.Clear();

            return shell;
        }

        protected abstract Type Command { get; }
    }
}
