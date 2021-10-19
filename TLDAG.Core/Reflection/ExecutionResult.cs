using System.Collections.Generic;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResult
    {
        private readonly List<string> outputs;
        private readonly List<string> errors;

        public int ExitCode { get; }
        public IReadOnlyList<string> Outputs { get => outputs; }
        public IReadOnlyList<string> Errors { get => errors; }

        public ExecutionResult(int exitCode, IEnumerable<string> outputs, IEnumerable<string> errors)
        {
            ExitCode = exitCode;

            this.outputs = new(outputs);
            this.errors = new(errors);
        }
    }
}
