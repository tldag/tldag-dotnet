using System;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Executing
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

        public ExecutionResult ThrowOnError()
        {
            if (ExitCode != 0 || Errors.Any())
                throw ExecutionFailed(ExitCode, Errors);

            return this;
        }
    }

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
