using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TLDAG.Core.Collections;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Strings;

namespace TLDAG.Core.IO
{
    public static partial class Files
    {
        public static readonly Compare<string> FileNameCompare = Platform.IsWindows ? CompareOrdinalIgnoreCase : CompareOrdinal;

        public static FileInfo Existing(string path) => File.Exists(path) ? new(path) : throw FileNotFound(path);
        public static FileInfo Existing(FileInfo file) => file.Exists ? file : throw FileNotFound(file);

        public static bool HasExtension(this FileInfo file, params string[] extensions)
            => HasExtension(file, new StringSet(extensions, FileNameCompare));

        public static bool HasExtension(this FileInfo file, StringSet extensions)
            => extensions.Contains(file.Extension) || extensions.Contains("");

        public static string NameWithoutExtension(this FileInfo file)
            => Path.GetFileNameWithoutExtension(file.Name) ?? throw FileNotFound(file);

        public static DirectoryInfo GetDirectory(this FileInfo file)
            => file.Directory ?? throw FileNotFound(file);

        public static string ReadAllText(this FileInfo file, Encoding? encoding = null)
            { encoding ??= Encoding.UTF8; return File.ReadAllText(file.FullName, encoding); }

        public static void WriteAllText(this FileInfo file, string text, Encoding? encoding = null)
            { encoding ??= Encoding.UTF8; File.WriteAllText(file.FullName, text, encoding); }

        public static byte[] ReadAllBytes(this FileInfo file) => File.ReadAllBytes(file.FullName);
        public static void WriteAllBytes(this FileInfo file, byte[] bytes) { File.WriteAllBytes(file.FullName, bytes); }

        public static Uri ToUri(this FileInfo file) => new(file.FullName);

        public static FileInfo FindOnPath(string pattern, bool prependWorkingDirectory = false)
        {
            if (TryFindOnPath(pattern, out FileInfo file, prependWorkingDirectory))
                return file;

            throw FileNotFound(pattern);
        }

        public static bool TryFindOnPath(string pattern, out FileInfo file, bool prependWorkingDirectory = false)
        {
#pragma warning disable CS8601 // Possible null reference assignment.

            file = Env.GetPath(prependWorkingDirectory)
                .SelectMany(dir => dir.GetFiles(pattern, TopDirectoryOnly))
                .FirstOrDefault();

#pragma warning restore CS8601 // Possible null reference assignment.

            return file is not null;
        }

        public static IEnumerable<FileInfo> FindAllOnPath(string pattern, bool prependWorkingDirectory = false)
            => Env.GetPath(prependWorkingDirectory).SelectMany(dir => dir.GetFiles(pattern, TopDirectoryOnly));
    }
}
