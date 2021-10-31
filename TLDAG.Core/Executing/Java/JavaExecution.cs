using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Core.Executing.Java
{
    public class JavaExecutionBuilder
    {
        private readonly ExecutionBuilder builder;
        private readonly FileInfo jarFile;
        private readonly List<string> arguments = new();

        public JavaExecutionBuilder(Executable executable, FileInfo jarFile)
        {
            builder = new(executable);
            this.jarFile = jarFile;

            builder.UseShellExecute(false).CreateNoWindow(true);
        }

        public static JavaExecutionBuilder Create(Executable executable, FileInfo jarFile)
            => new(executable, jarFile);

        public JavaExecutionBuilder WorkingDirectory(DirectoryInfo directory)
            { builder.WorkingDirectory(directory); return this; }

        public JavaExecutionBuilder AddArgument(string arg) { arguments.Add(arg); return this; }

        public JavaExecutionBuilder AddArguments(params string[] args)
            { foreach (string arg in args) AddArgument(arg); return this; }

        public JavaExecutionBuilder AddArguments(IEnumerable<string> args)
            { foreach (string arg in args) AddArgument(arg); return this; }

        public Execution Build()
        {
            builder.AddArguments("-jar", $"{jarFile.FullName}");
            builder.AddArguments(arguments);

            return builder.Build();
        }
    }
}
