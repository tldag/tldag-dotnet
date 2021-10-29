using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TLDAG.Core.Executing
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

        public ExecutionResult Execute()
        {
            return Start().Result;
        }

        public async Task<ExecutionResult> Start()
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

            await WaitForExitAsync(process);

            builder.ExitCode(process.ExitCode);
            builder.Started(startTime);
            builder.Exited(DateTime.UtcNow);

            return builder.Build();
        }

        private async Task WaitForExitAsync(Process process)
        {
#if NET5_0_OR_GREATER
            await process.WaitForExitAsync();
#else
            while (!process.HasExited)
                await Task.Delay(10);
#endif
        }
    }

        public class ExecutionBuilder
    {
        private readonly Executable executable;
        private readonly ProcessStartInfo info;
        private readonly List<string> arguments = new();

        public ExecutionBuilder(Executable executable)
        {
            this.executable = executable;
            info = new(executable.Path);
        }

        public static ExecutionBuilder Create(Executable executable) => new(executable);
        public static ExecutionBuilder Create(string name) => Create(Executables.Find(name));

        public ExecutionBuilder UseShellExecute(bool use) { info.UseShellExecute = use; return this; }
        public ExecutionBuilder CreateNoWindow(bool noWindow) { info.CreateNoWindow = noWindow; return this; }
        public ExecutionBuilder WorkingDirectory(DirectoryInfo directory) { info.WorkingDirectory = directory.FullName; return this; }

        public ExecutionBuilder SetEnvironmentVariable(string key, string value)
        {
            if (info.Environment.ContainsKey(key))
                info.Environment.Remove(key);

            if (info.EnvironmentVariables.ContainsKey(key))
                info.EnvironmentVariables.Remove(key);

            info.Environment[key] = value;
            info.EnvironmentVariables[key] = value;

            return this;
        }

        public ExecutionBuilder SetEnvironmentVariables(IEnumerable<KeyValuePair<string, string>> variables)
        {
            foreach (KeyValuePair<string, string> variable in variables)
                SetEnvironmentVariable(variable.Key, variable.Value);

            return this;
        }

        public ExecutionBuilder AddArgument(string arg) { arguments.Add(arg); return this; }

        public ExecutionBuilder AddArguments(params string[] args)
        { foreach (string arg in args) AddArgument(arg); return this; }

        public ExecutionBuilder AddArguments(IEnumerable<string> args)
        { foreach (string arg in args) AddArgument(arg); return this; }

        public Execution Build() { ProcessArguments(); return new(executable, info); }

#if NET472
        private void ProcessArguments() { info.Arguments = string.Join(" ", arguments); }
#endif

#if NET5_0_OR_GREATER
        private void ProcessArguments()
            { foreach (string arg in arguments) info.ArgumentList.Add(arg); }
#endif
    }
}
