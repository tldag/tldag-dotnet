using System;
using System.IO;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.IO
{
    public static class Streams
    {
        public static void Write(this Stream stream, byte[] bytes)
            { stream.Write(bytes, 0, bytes.Length); }
    }
}