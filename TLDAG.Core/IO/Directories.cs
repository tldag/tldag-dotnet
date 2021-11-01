using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.IO
{
    public static class Directories
    {
        public static bool DirectoryExists(DirectoryInfo? directory)
            => directory?.Exists ?? false;

        public static DirectoryInfo? TryGetDirectory(string? path)
            => path is null ? null : (Directory.Exists(path) ? new(path) : null);

#pragma warning disable CS8601 // Possible null reference assignment.
        public static bool TryGetDirectory(string? path, out DirectoryInfo directory)
            => (directory = TryGetDirectory(path)) is not null;
#pragma warning restore CS8601 // Possible null reference assignment.

        public static DirectoryInfo ExistingDirectory(string? path)
            => TryGetDirectory(path) ?? throw DirectoryNotFound(path ?? "");

        public static DirectoryInfo ExistingDirectory(DirectoryInfo? directory)
        {
            if (directory is null) throw DirectoryNotFound("");
            return directory.Exists ? directory : throw DirectoryNotFound(directory.FullName);
        }

        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directory, IEnumerable<string> patterns)
            => patterns.SelectMany(pattern => directory.EnumerateFiles(pattern));

        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directory, IEnumerable<string> patterns, SearchOption searchOption)
            => patterns.SelectMany(pattern => directory.EnumerateFiles(pattern, searchOption));

        public static FileInfo GetFileAbove(this DirectoryInfo startDirectory, string fileName)
        {
            DirectoryInfo? directory = startDirectory;

            while (directory is not null)
            {
                FileInfo? file = directory.EnumerateFiles(fileName, TopDirectoryOnly).FirstOrDefault();

                if (file is not null) return file;

                directory = directory.Parent;
            }

            throw FileNotFound(fileName);
        }

        public static DirectoryInfo GetDirectoryOfFileAbove(this DirectoryInfo startDirectory, string fileName)
            => GetFileAbove(startDirectory, fileName).GetDirectory();

        public static void Clear(this DirectoryInfo root)
        {
            foreach(DirectoryInfo directory in root.EnumerateDirectories())
            {
                if (Directory.Exists(directory.FullName))
                    directory.Delete(true);
            }

            foreach (FileInfo file in root.EnumerateFiles())
            {
                if (File.Exists(file.FullName))
                    file.Delete();
            }
        }

        public static Uri ToUri(this DirectoryInfo directory)
            => new(directory.FullName + "/");
    }
}
