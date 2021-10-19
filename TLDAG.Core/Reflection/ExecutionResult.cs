using System.Collections.Generic;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResult
    {
        private readonly List<string> output;
        private readonly List<string> error;

        public int ExitCode { get; }
        public IReadOnlyList<string> Output { get => output; }
        public IReadOnlyList<string> Error { get => error; }

        public ExecutionResult(int exitCode, IEnumerable<string> output, IEnumerable<string> error)
        {
            ExitCode = exitCode;

            this.output = new(output);
            this.error = new(error);
        }
    }
}
