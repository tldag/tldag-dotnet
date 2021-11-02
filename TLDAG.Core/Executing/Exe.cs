using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Strings;

namespace TLDAG.Core.Executing
{
    public class Executable
    {
        public static readonly StringSet ExecutableExtensions
            = Platform.IsWindows ? new(".exe", Strings.CompareOrdinalIgnoreCase) : new("");

        public FileInfo File { get; }
        public DirectoryInfo Directory { get => File.GetDirectory(); }
        public string Path { get => File.FullName; }

        public Executable(FileInfo file)
        {
            File = file;
        }

        public static IEnumerable<Executable> FindAll(string name)
            => ExecutableExtensions.Select(ext => name + ext).SelectMany(Files.FindAllOnPath).Select(file => new Executable(file));

        public static bool TryFind(string name, out Executable executable)
            => Out.Set(FindAll(name).FirstOrDefault(), out executable);

        public static Executable Find(string name)
            => FindAll(name).FirstOrDefault() ?? throw FileNotFound(name);
    }

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

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"ExitCode = {ExitCode}");
            sb.Append(NewLine);
            sb.Append($"ElapsedTime = {ElapsedTime}");

            if (Errors.Any())
            {
                sb.Append(NewLine).Append("Errors:").Append(NewLine);
                sb.Append(string.Join(NewLine, Errors));
            }

            if (Outputs.Any())
            {
                sb.Append(NewLine).Append("Output:").Append(NewLine);
                sb.Append(string.Join(NewLine, Outputs));
            }

            return sb.ToString();
        }
    }

    public class ExecutionResultBuilder
    {
        private readonly Executable executable;
        private readonly List<string> outputs = new();
        private readonly List<string> errors = new();
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

        public ExecutionResult Execute(CancellationToken cancellationToken = default)
            => Start(cancellationToken).Result;

        public async Task<ExecutionResult> Start(CancellationToken cancellationToken = default)
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

            await process.WaitExit(cancellationToken);

            builder.ExitCode(process.ExitCode);
            builder.Started(startTime);
            builder.Exited(DateTime.UtcNow);

            return builder.Build();
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
        public static ExecutionBuilder Create(string name) => Create(Executable.Find(name));

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

        public Execution Build() => new(executable, info.SetArguments(arguments));
    }
}
