using System;
using System.Runtime.Serialization;

namespace TLDAG.Core
{
    public class InvalidStateExcepion : Exception, ISerializable
    {
        public InvalidStateExcepion() { }
        public InvalidStateExcepion(string message) : base(message) { }
        protected InvalidStateExcepion(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
