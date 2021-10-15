using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core.Algorithms;

namespace TLDAG.Core.IO
{
    public static class Paths
    {
        public static FileInfo Combine(this DirectoryInfo directory, string first, params string[] more)
            => new(CombinePath(directory, first, more));

        public static DirectoryInfo CombineDirectory(this DirectoryInfo directory, string first, params string[] more)
            => new(CombinePath(directory, first, more));

        private static string CombinePath(DirectoryInfo directory, string first, params string[] more)
        {
            string[] paths = new string[2 + more.Length];

            paths[0] = directory.FullName;
            paths[1] = first;
            Arrays.Replace(paths, 2, more, 0, more.Length);

            return Path.Combine(paths);
        }
    }
}
