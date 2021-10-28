using System;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Reflection
{
    public class ExecutionResult
    {
        private readonly List<string> outputs;
        private readonly List<string> errors;

        public Executable Executable { get; }
        public int ExitCode { get; }

        public DateTime StartTime { get; }
        public DateTime ExitTime { get; }
        public TimeSpan ElapsedTime { get => ExitTime - StartTime; }

        public IReadOnlyList<string> Outputs { get => outputs; }
        public IReadOnlyList<string> Errors { get => errors; }

        public ExecutionResult(Executable executable, int exitCode, DateTime startTime, DateTime exitTime,
            IEnumerable<string> outputs, IEnumerable<string> errors)
        {
            Executable = executable;
            ExitCode = exitCode;

            StartTime = startTime;
            ExitTime = exitTime;

            this.outputs = new(outputs);
            this.errors = new(errors);
        }

        public void ThrowOnError()
        {
            if (ExitCode != 0 || Errors.Any())
                throw ExecutionFailed(ExitCode, Errors);
        }
    }
}
