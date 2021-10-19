using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static TLDAG.Core.Resources.ErrorsResources;

namespace TLDAG.Core.Exceptions
{
    public class ExecutionException : ApplicationException, ISerializable
    {
        public ExecutionException() { }
        public ExecutionException(int exitCode, IEnumerable<string> lines) : base(Format(exitCode, lines)) { }
        protected ExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        private static string Format(int exitCode, IEnumerable<string> lines)
            => ExecutionExceptionFormat.Format(exitCode, string.Join("\n", lines));
    }
}
