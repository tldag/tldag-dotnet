using NuGet.Versioning;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core;
using TLDAG.Core.Executing;
using static TLDAG.Build.Resources.DotNetResources;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.DotNet
{
    public class DotNetInfo
    {
        public Executable Executable { get; }
        public SemanticVersion Version { get; }
        public DirectoryInfo BaseDirectory { get; }

        private DotNetInfo(Executable executable, SemanticVersion version, DirectoryInfo baseDirectory)
        {
            Executable = executable;
            Version = version;
            BaseDirectory = baseDirectory;
        }

        public static DotNetInfo Get(DirectoryInfo directory)
        {
            ExecutionResult result = ExecutionBuilder.Create("dotnet")
                .UseShellExecute(false).CreateNoWindow(true)
                .WorkingDirectory(directory)
                .AddArgument("--info")
                .Build().Execute().ThrowOnError();

            List<string> trimmed = result.Outputs.Select(line => line.Trim()).ToList();
            SemanticVersion version = ParseVersion(trimmed);
            DirectoryInfo baseDirectory = ParseBaseDirectory(trimmed);

            return new(result.Executable, version, baseDirectory);
        }

        public const string VersionLinePrefix = "Version:";

        private static SemanticVersion ParseVersion(IEnumerable<string> lines)
        {
            string? versionLine = lines
                .Where(line => line.StartsWith(VersionLinePrefix, OrdinalComparison))
                .FirstOrDefault();

            string parseable = Contract.State.NotNull<string>(versionLine, DotNetInfoNoVersion)
                .Substring(VersionLinePrefix.Length).Trim();

            return SemanticVersion.Parse(parseable);
        }

        public const string BasePathPrefix = "Base Path:";

        private static DirectoryInfo ParseBaseDirectory(IEnumerable<string> lines)
        {
            string? basePathLine = lines
                .Where(line => line.StartsWith(BasePathPrefix, OrdinalComparison))
                .FirstOrDefault();

            string parseable = Contract.State.NotNull<string>(basePathLine, DotNetInfoNoBasePath)
                .Substring(BasePathPrefix.Length).Trim();

            return new(parseable);
        }
    }
}
