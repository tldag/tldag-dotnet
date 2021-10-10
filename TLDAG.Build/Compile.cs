using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build
{
    public static class Compile
    {
        public static readonly StringSet CSharpExtensions = new(".cs");

        public static IEnumerable<FileInfo> GetCompileFiles(FileInfo listFile)
        {
            return File
                .ReadAllLines(listFile.FullName)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(path => new FileInfo(path))
                .Where(file => file.Exists);
        }

        public static IEnumerable<FileInfo> GetCSharpFiles(FileInfo listFile)
            => GetCSharpFiles(GetCompileFiles(listFile));

        public static IEnumerable<FileInfo> GetCSharpFiles(IEnumerable<FileInfo> files)
            => files.Where(file => file.HasExtension(CSharpExtensions));
    }
}
