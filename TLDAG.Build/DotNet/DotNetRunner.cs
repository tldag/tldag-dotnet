using System.IO;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;

namespace TLDAG.Build.DotNet
{
    public class DotNetRunner
    {
        public static ExecutionResult Restore(FileInfo slnOrProj, DotNetOptions options, bool throwOnError)
            => Execute("restore", slnOrProj, options, throwOnError);

        public static ExecutionResult Execute(string command, FileInfo slnOrProj, DotNetOptions options, bool throwOnError)
            => AddOptions(CreateBuilder(command, slnOrProj), options).Build().Execute(throwOnError);

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
