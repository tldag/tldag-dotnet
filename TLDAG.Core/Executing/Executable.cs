using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Executing
{
    public class Executable
    {
        public FileInfo File { get; }
        public DirectoryInfo Directory { get => File.GetDirectory(); }
        public string Path { get => File.FullName; }

        public Executable(FileInfo file)
        {
            File = file;
        }
    }

    public static class Executables
    {
        private static readonly StringSet ExecutableExtensions
            = Platform.IsWindows ? new(".exe", Strings.CompareOrdinalIgnoreCase) : new("");

        public static Executable Find(string name)
        {
            if (TryFind(name, out Executable executable))
                return executable;

            throw FileNotFound(name);
        }

        public static bool TryFind(string name, out Executable executable)
        {
#pragma warning disable CS8601 // Possible null reference assignment.

            executable = LookupPath
                .Select(directory => Find(directory, name))
                .Where(e => e is not null)
                .FirstOrDefault();

#pragma warning restore CS8601 // Possible null reference assignment.

            return executable is not null;
        }

        public static Executable? Find(DirectoryInfo directory, string name, bool deep = false)
        {
            SearchOption options = deep ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            return ExecutableExtensions
                .Select(ext => name + ext)
                .SelectMany(pattern => directory.GetFiles(pattern, options))
                .Select(file => new Executable(file))
                .FirstOrDefault();
        }

        public static IEnumerable<DirectoryInfo> LookupPath => Env.Path.Prepend(Env.WorkingDirectory);
    }

}
