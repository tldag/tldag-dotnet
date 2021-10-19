using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using static TLDAG.Core.Resources.ErrorsResources;

namespace TLDAG.Core.Exceptions
{
    public static class Errors
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

        public static DirectoryNotFoundException DirectoryNotFound(string path) => new(path);
        public static DirectoryNotFoundException DirectoryNotFound(DirectoryInfo directory) => new(directory.FullName);

        public static IOException BadFileFormat(string message, FileInfo file)
            => new(BadFileFormatFormat.Format(message, file.FullName));

        public static ArgumentException InvalidArgument(string paramName, string message) => new(message, paramName);

        public static ArgumentOutOfRangeException OutOfRange(string paramName, object actualValue, string message)
            => new(paramName, actualValue, message);

        public static InvalidStateExcepion InvalidState(string message)
            => new(InvalidStateFormat.Format(message));

        public static ExecutionException ExecutionFailed(int exitCode, IEnumerable<string> errors)
            => new(exitCode, errors);
    }
}
