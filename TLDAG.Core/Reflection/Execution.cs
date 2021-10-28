using System;
using System.Diagnostics;
using System.Linq;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Reflection
{
    public class Execution
    {
        private readonly Executable executable;
        private readonly ProcessStartInfo startInfo;

        public Execution(Executable executable, ProcessStartInfo startInfo)
        {
            this.executable = executable;
            this.startInfo = startInfo;

            startInfo.FileName = executable.Path;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
        }

        public ExecutionResult Execute(bool throwOnError)
        {
            using Process process = new();
            ExecutionResultBuilder builder = new(executable);
            DateTime startTime = DateTime.UtcNow;

            process.StartInfo = startInfo;
            process.OutputDataReceived += (_, e) => builder.Output(e.Data);
            process.ErrorDataReceived += (_, e) => builder.Error(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            builder.ExitCode(process.ExitCode);
            builder.Started(startTime);
            builder.Exited(DateTime.UtcNow);

            ExecutionResult result = builder.Build();

            if (throwOnError)
                result.ThrowOnError();

            return result;
        }
    }
}
