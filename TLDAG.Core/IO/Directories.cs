using System;
using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.IO
{
    public static class Directories
    {
        public static DirectoryInfo Existing(string path)
            => Directory.Exists(path) ? new(path) : throw DirectoryNotFound(path);

        public static DirectoryInfo Existing(DirectoryInfo? directory)
        {
            if (directory == null) throw DirectoryNotFound("");
            return directory.Exists ? directory : throw DirectoryNotFound(directory.FullName);
        }

        public static FileInfo GetFileAbove(this DirectoryInfo startDirectory, string fileName)
        {
            DirectoryInfo? directory = startDirectory;

            while (directory != null)
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
