using System;
using System.IO;
using System.Text;
using TLDAG.Core.Collections;
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
    }
}
