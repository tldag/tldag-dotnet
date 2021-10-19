using System.IO;
using TLDAG.Core.IO;

namespace TLDAG.Core.Reflection
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
}
