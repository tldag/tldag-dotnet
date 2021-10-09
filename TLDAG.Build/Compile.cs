using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TLDAG.Build
{
    public static class Compile
    {
        public static IEnumerable<FileInfo> GetCompileFiles(FileInfo listFile)
        {
            return File
                .ReadAllLines(listFile.FullName)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(path => new FileInfo(path))
                .Where(file => file.Exists);
        }
    }
}
