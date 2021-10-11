using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TLDAG.Core
{
    public static class Exceptions
    {
        public static Exception InnerMost(this Exception exception)
            => exception.InnerException?.InnerMost() ?? exception;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static NotImplementedException NotYetImplemented() => new(new StackFrame(1, true).ToString());

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static NotSupportedException NotSupported() => new(new StackFrame(1, true).ToString());

        public static ObjectDisposedException ObjectDisposed(string? name = null) => new(name);

        public static FileNotFoundException FileNotFound(string path) => new(null, path);
        public static FileNotFoundException FileNotFound(FileInfo file) => new(null, file.FullName);

        public static ArgumentException InvalidArgument(string paramName, string message) => new(message, paramName);

        public static ArgumentOutOfRangeException OutOfRange(string paramName, object actualValue, string message)
            => new(paramName, actualValue, message);
    }
}
