using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using TLDAG.Core.IO;

namespace TLDAG.Automation
{
    public class Shell
    {
        private readonly PowerShell shell;
        private readonly HashSet<FileInfo> imported = new();

        public Shell() { shell = PowerShell.Create(); }
        public Shell(Type commandType) : this() { ImportModule(commandType); }

        public void Clear() { shell.Commands.Clear(); shell.Streams.ClearStreams(); }

        public CommandResults Invoke(string script)
            { Clear(); shell.AddScript(script); return new(shell, shell.Invoke()); }

        public CommandResults<T> Invoke<T>(string script)
            { Clear(); shell.AddScript(script); return new(shell, shell.Invoke<T>()); }

        public void ImportModule(Type commandType) { ImportModule(Files.Existing(commandType.Assembly.Location));  }

        public void ImportModule(FileInfo file)
        {
            if (imported.Contains(file)) return;
            Invoke($"Import-Module '{file.FullName}'").ThrowExceptions(true);
            imported.Add(file);
        }
    }
}
