using System.IO;
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
            foreach (string ext in ExecutableExtensions)
            {
                if (Files.TryFindOnPath(name + ext, out FileInfo file, true))
                {
                    executable = new(file);
                    return true;
                }
            }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            executable = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            return false;
        }
    }
}
