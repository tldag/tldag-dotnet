using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TLDAG.Core.Reflection
{
    public class ExecutionBuilder
    {
        private readonly ProcessStartInfo info;
        private readonly List<string> arguments = new();

        public ExecutionBuilder(Executable executable)
        {
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

        public Execution Build() { ProcessArguments(); return new(info);}

#if NET472
        private void ProcessArguments() { info.Arguments = string.Join(" ", arguments); }
#endif

#if NET5_0_OR_GREATER
        private void ProcessArguments()
            { foreach (string arg in arguments) info.ArgumentList.Add(arg); }
#endif
    }
}
