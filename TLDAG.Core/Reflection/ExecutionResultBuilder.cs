using System.Collections.Generic;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResultBuilder
    {
        private List<string> output = new();
        private List<string> error = new();
        private int exitCode;

        public ExecutionResultBuilder Output(string? data)
            { if (data is not null) output.Add(data); return this; }

        public ExecutionResultBuilder Error(string? data)
            { if (data is not null) error.Add(data); return this; }

        public ExecutionResultBuilder ExitCode(int exitCode) { this.exitCode = exitCode; return this; }

        public ExecutionResult Build() => new(exitCode, output, error);
    }
}
