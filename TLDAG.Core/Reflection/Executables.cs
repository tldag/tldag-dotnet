using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Reflection
{
    public static class Executables
    {
        private static readonly StringSet ExecutableExtensions
            = Env.IsWindows ? new(".exe", Strings.CompareOrdinalIgnoreCase) : new("");

        public static Executable Find(string name)
        {
            return LookupPath
                .Select(directory => Find(directory, name))
                .Where(e => e is not null)
                .FirstOrDefault() ?? throw FileNotFound(name);
        }

        public static Executable? Find(DirectoryInfo directory, string name, bool deep = false)
        {
            SearchOption options = deep ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            return ExecutableExtensions
                .Select(ext => name + ext)
                .SelectMany(pattern => directory.GetFiles(pattern, options))
                .Select(file => new Executable(file.FullName))
                .FirstOrDefault();
        }

        public static IEnumerable<DirectoryInfo> LookupPath => Env.Path.Prepend(Env.CurrentDirectory);
    }
}
