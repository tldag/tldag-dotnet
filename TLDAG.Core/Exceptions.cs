using System;
using System.IO;

namespace TLDAG.Core
{
    public static class Exceptions
    {
        public static NotImplementedException NotYetImplemented(string method) => new(method);

        public static FileNotFoundException FileNotFound(string path) => new(null, path);
        public static FileNotFoundException FileNotFound(FileInfo file) => new(null, file.FullName);
    }
}
