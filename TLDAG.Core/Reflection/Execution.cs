using System.Diagnostics;
using static TLDAG.Core.Exceptions;

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

        public ExecutionResult Execute()
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

            return builder.Build();
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw NotYetImplemented();
        }
    }
}
