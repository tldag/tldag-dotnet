using System.IO;
using TLDAG.Core.Executing;
using TLDAG.Core.IO;

namespace TLDAG.Build.DotNet
{
    public class DotNetRunner
    {
        public static ExecutionResult Build(FileInfo slnOrProj, DotNetOptions options)
            => Execute("build", slnOrProj, options);

        public static ExecutionResult Restore(FileInfo slnOrProj, DotNetOptions options)
            => Execute("restore", slnOrProj, options);

        public static ExecutionResult Execute(string command, FileInfo slnOrProj, DotNetOptions options)
            => AddOptions(CreateBuilder(command, slnOrProj), options).Build().Execute();

        private static ExecutionBuilder CreateBuilder(string command, FileInfo slnOrProj)
        {
            return ExecutionBuilder.Create("dotnet")
                .UseShellExecute(false).CreateNoWindow(true)
                .WorkingDirectory(slnOrProj.GetDirectory())
                .AddArgument(command).AddArgument($"\"{slnOrProj.FullName}\"");
        }

        private static ExecutionBuilder AddOptions(ExecutionBuilder builder, DotNetOptions options)
        {
            options.Loggers.ForEach(info => { builder.AddArgument($"-logger:{info}"); });

            return builder;
        }
    }
}
