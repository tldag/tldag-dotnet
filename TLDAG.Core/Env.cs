using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TLDAG.Core
{
    public static class Env
    {
        public static readonly char PathSeparator = Path.PathSeparator;

        public static DirectoryInfo WorkingDirectory => new(Environment.CurrentDirectory);

        public static IEnumerable<DirectoryInfo> GetPath(bool prependWorkingDirectory = false)
        {
            IEnumerable<DirectoryInfo> path =
                (Environment.GetEnvironmentVariable("PATH") ?? "")
                .Split(PathSeparator)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new DirectoryInfo(s))
                .Where(d => d.Exists);

            if (prependWorkingDirectory)
                path = path.Prepend(WorkingDirectory);

            return path;
        }
    }
}
