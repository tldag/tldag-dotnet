using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Strings;

namespace TLDAG.Core.IO
{
    public static class Files
    {
        public static readonly Compare<string> FileNameCompare = Env.IsWindows ? CompareOrdinalIgnoreCase : CompareOrdinal;

        public static FileInfo Existing(string path) => File.Exists(path) ? new(path) : throw FileNotFound(path);
        public static FileInfo Existing(FileInfo file) => file.Exists ? file : throw FileNotFound(file);

        public static bool HasExtension(this FileInfo file, params string[] extensions)
            => HasExtension(file, new StringSet(extensions, FileNameCompare));

        public static bool HasExtension(this FileInfo file, StringSet extensions)
            => extensions.Contains(file.Extension);
    }
}
