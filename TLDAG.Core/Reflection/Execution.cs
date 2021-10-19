using System.Diagnostics;
using System.Linq;

namespace TLDAG.Core.Reflection
{
    public class Execution
    {
        private ProcessStartInfo startInfo;

        public Execution(ProcessStartInfo info)
        {
            startInfo = info;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
        }

        public ExecutionResult Execute(bool throwOnError)
        {
            using Process process = new();
            ExecutionResultBuilder builder = new();

            process.StartInfo = startInfo;
            process.OutputDataReceived += (_, e) => builder.Output(e.Data);
            process.ErrorDataReceived += (_, e) => builder.Error(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            builder.ExitCode(process.ExitCode);

            ExecutionResult result = builder.Build();

            if (throwOnError && (result.ExitCode != 0 || result.Error.Any()))
            {

            }

            return result;
        }
    }
}
