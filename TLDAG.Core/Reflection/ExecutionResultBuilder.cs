using System.Collections.Generic;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResultBuilder
    {
        private Executable executable;
        private List<string> outputs = new();
        private List<string> errors = new();
        private int exitCode;

        public ExecutionResultBuilder(Executable executable) { this.executable = executable; }

        public static ExecutionResultBuilder Create(Executable executable) => new(executable);

        public ExecutionResultBuilder Output(string? data)
            { if (data is not null) outputs.Add(data); return this; }

        public ExecutionResultBuilder Error(string? data)
            { if (data is not null) errors.Add(data); return this; }

        public ExecutionResultBuilder ExitCode(int exitCode) { this.exitCode = exitCode; return this; }

        public ExecutionResult Build() => new(executable, exitCode, outputs, errors);
    }
}
