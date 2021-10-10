using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core
{
    public static class Env
    {
        public static readonly char PathSeparator = System.IO.Path.PathSeparator;

        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool IsOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static DirectoryInfo CurrentDirectory => new(Environment.CurrentDirectory);

        public static IEnumerable<DirectoryInfo> Path
        {
            get
            {
                return (Environment.GetEnvironmentVariable("PATH") ?? "")
                    .Split(PathSeparator)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => new DirectoryInfo(s))
                    .Where(d => d.Exists);
            }
        }
    }
}
