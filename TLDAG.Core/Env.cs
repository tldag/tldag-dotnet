using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using static System.IO.Path;
using static TLDAG.Core.IO.Directories;

namespace TLDAG.Core
{
    public static class Env
    {
        public static DirectoryInfo WorkingDirectory => new(Environment.CurrentDirectory);

        public static string GetEnvironmentVariable(string variableName)
            => Environment.GetEnvironmentVariable(variableName) ?? "";

        public static IEnumerable<DirectoryInfo> SplitPath(string value)
            => value.Split(PathSeparator).Select(TryGetDirectory).NotNull();

        public static IEnumerable<DirectoryInfo> Path
            { get => SplitPath(GetEnvironmentVariable("PATH")).Prepend(WorkingDirectory); }

        public static DirectoryInfo? GetDirectory(string variableName)
            => TryGetDirectory(GetEnvironmentVariable(variableName));
    }
}
