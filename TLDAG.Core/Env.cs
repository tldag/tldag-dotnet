using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core
{
    public static class Env
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static IEnumerable<DirectoryInfo> Path() => throw NotYetImplemented();
    }
}
