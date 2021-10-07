using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.IO
{
    public static class Files
    {
        public static FileInfo Existing(string path) => File.Exists(path) ? new(path) : throw FileNotFound(path);
        public static FileInfo Existing(FileInfo file) => file.Exists ? file : throw FileNotFound(file);
    }
}
