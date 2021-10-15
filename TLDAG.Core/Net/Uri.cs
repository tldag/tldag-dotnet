using System;
using System.IO;

namespace TLDAG.Core.Net
{
    public static class UriExtensions
    {
        public static FileInfo ToFile(this Uri uri)
        {
            Contract.Arg.Condition(uri.IsFile, nameof(uri), $"Not a file '{uri}'");
            return new(uri.LocalPath);
        }

        public static DirectoryInfo ToDirectory(this Uri uri)
        {
            Contract.Arg.Condition(uri.IsFile, nameof(uri), $"Not a directory '{uri}'");
            return new(uri.LocalPath);
        }
    }
}