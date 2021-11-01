using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.Path;

namespace TLDAG.Core
{
    public static class Env
    {
        public static DirectoryInfo WorkingDirectory => new(Environment.CurrentDirectory);

        public static IEnumerable<DirectoryInfo> GetPath()
        {
            IEnumerable<DirectoryInfo> path =
                (Environment.GetEnvironmentVariable("PATH") ?? "")
                .Split(PathSeparator)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new DirectoryInfo(s))
                .Where(d => d.Exists);

            path = path.Prepend(WorkingDirectory);

            return path;
        }

        public static DirectoryInfo? GetDirectory(string variableName)
        {
            string? path = Environment.GetEnvironmentVariable(variableName);

            if (path is null || string.IsNullOrWhiteSpace(path))
                return null;

            return Directory.Exists(path) ? new(path) : null;
        }
    }
}
