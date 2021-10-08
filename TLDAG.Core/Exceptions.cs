using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TLDAG.Core
{
    public static class Exceptions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static NotImplementedException NotYetImplemented() => new(new StackFrame(1, true).ToString());

        public static FileNotFoundException FileNotFound(string path) => new(null, path);
        public static FileNotFoundException FileNotFound(FileInfo file) => new(null, file.FullName);
    }
}
