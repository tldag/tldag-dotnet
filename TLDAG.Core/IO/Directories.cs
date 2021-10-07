﻿using System.IO;
using System.Linq;
using static System.IO.SearchOption;

namespace TLDAG.Core.IO
{
    public static class Directories
    {
        public static void Validate(FileInfo file)
        {
            DirectoryInfo? directory = file.Directory;

            if (directory is null) throw new DirectoryNotFoundException();
            if (!directory.Exists) throw new DirectoryNotFoundException();
        }

        public static DirectoryInfo GetDirectoryOfFileAbove(string startDirectory, string fileName)
            => GetDirectoryOfFileAbove(new DirectoryInfo(startDirectory), fileName);

        public static DirectoryInfo GetDirectoryOfFileAbove(DirectoryInfo startDirectory, string fileName)
        {
            DirectoryInfo? directory = startDirectory;

            while (directory != null)
            {
                if (directory.EnumerateFiles(fileName, TopDirectoryOnly).Any())
                    return directory;

                directory = directory.Parent;
            }

            throw new FileNotFoundException();
        }
    }
}
