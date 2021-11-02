using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Executing
{
    public static class Java
    {
        public static readonly string JavaExecutableName = Platform.IsWindows ? "java.exe" : "java";

        public static DirectoryInfo? GetSdkDirectory() => Env.GetDirectory("JAVA_HOME");
        public static DirectoryInfo? GetJreDirectory() => Env.GetDirectory("JRE_HOME");

        public static bool HasJava { get => FindAll().Any(); }

        public static IEnumerable<DirectoryInfo> JavaEnvDirectories()
        {
            if (GetSdkDirectory() is DirectoryInfo sdkDirectory) yield return sdkDirectory;
            if (GetJreDirectory() is DirectoryInfo jreDirectory) yield return jreDirectory;
        }

        public static IEnumerable<Executable> FindAll()
        {
            foreach (DirectoryInfo directory in JavaEnvDirectories())
            {
                foreach (FileInfo file in directory.EnumerateFiles(JavaExecutableName, AllDirectories))
                    yield return new Executable(file);
            }

            foreach (Executable executable in Executable.FindAll("java"))
                yield return executable;
        }

        public static bool TryFind(out Executable executable)
            => Out.Set(FindAll().FirstOrDefault(), out executable);

        public static Executable Find()
            => FindAll().FirstOrDefault() ?? throw FileNotFound(JavaExecutableName);

        public static ExecutionResult Execute(FileInfo jarFile, params string[] args)
            => JavaExecutionBuilder.Create(Find(), jarFile).AddArguments(args).Build().Execute();
    }

    public class JavaExecutionBuilder
    {
        private readonly ExecutionBuilder builder;
        private readonly FileInfo jarFile;
        private readonly List<string> arguments = new();

        public JavaExecutionBuilder(Executable executable, FileInfo jarFile)
        {
            builder = new(executable);
            this.jarFile = jarFile;

            builder.UseShellExecute(false).CreateNoWindow(true);
        }

        public static JavaExecutionBuilder Create(Executable executable, FileInfo jarFile)
            => new(executable, jarFile);

        public JavaExecutionBuilder WorkingDirectory(DirectoryInfo directory)
            { builder.WorkingDirectory(directory); return this; }

        public JavaExecutionBuilder AddArgument(string arg) { arguments.Add(arg); return this; }

        public JavaExecutionBuilder AddArguments(params string[] args)
            { foreach (string arg in args) AddArgument(arg); return this; }

        public JavaExecutionBuilder AddArguments(IEnumerable<string> args)
            { foreach (string arg in args) AddArgument(arg); return this; }

        public Execution Build()
        {
            builder.AddArguments("-jar", $"{jarFile.FullName}");
            builder.AddArguments(arguments);

            return builder.Build();
        }
    }
}
