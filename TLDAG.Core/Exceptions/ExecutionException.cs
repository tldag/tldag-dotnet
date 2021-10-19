using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static TLDAG.Core.Resources.ErrorsResources;
using static TLDAG.Core.Strings;

namespace TLDAG.Core.Exceptions
{
    public class ExecutionException : ApplicationException, ISerializable
    {
        public ExecutionException() { }
        public ExecutionException(int exitCode, IEnumerable<string> errors) : base(Format(exitCode, errors)) { }
        protected ExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        private static string Format(int exitCode, IEnumerable<string> errors)
            => ExecutionExceptionFormat.Format(exitCode, NewLine, errors.Join(NewLine));
    }
}
