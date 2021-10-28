using System;
using System.Collections.Generic;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResultBuilder
    {
        private Executable executable;
        private List<string> outputs = new();
        private List<string> errors = new();
        private int exitCode;
        private DateTime startTime = DateTime.UtcNow;
        private DateTime exitTime = DateTime.UtcNow;

        public ExecutionResultBuilder(Executable executable) { this.executable = executable; }

        public static ExecutionResultBuilder Create(Executable executable) => new(executable);

        public ExecutionResultBuilder Output(string? data)
            { if (data is not null) outputs.Add(data); return this; }

        public ExecutionResultBuilder Error(string? data)
            { if (data is not null) errors.Add(data); return this; }

        public ExecutionResultBuilder ExitCode(int exitCode) { this.exitCode = exitCode; return this; }
        public ExecutionResultBuilder Started(DateTime startTime) { this.startTime = startTime; return this; }
        public ExecutionResultBuilder Exited(DateTime exitTime) { this.exitTime = exitTime; return this; }

        public ExecutionResult Build() => new(executable, exitCode, startTime, exitTime, outputs, errors);
    }
}
