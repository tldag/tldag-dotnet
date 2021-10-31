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
        public static readonly StringSet ExecutableExtensions
            = Platform.IsWindows ? new(".exe", Strings.CompareOrdinalIgnoreCase) : new("");

        public static IEnumerable<Executable> FindAllExecutables(string name)
        {
            foreach (string ext in ExecutableExtensions)
            {
                foreach (FileInfo file in Files.FindAllOnPath(name + ext, true))
                    yield return new(file);
            }
        }

        public static bool TryFindExecutable(string name, out Executable executable)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            executable = FindAllExecutables(name).FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.

            return executable is not null;
        }

        public static Executable FindExecutable(string name)
        {
            if (TryFindExecutable(name, out Executable executable))
                return executable;

            throw FileNotFound(name);
        }
    }
}
