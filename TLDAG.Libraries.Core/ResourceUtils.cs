using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core
{
    public static class ResourceUtils
    {
        public static Stream GetStreamFromResource(byte[] bytes, bool compressed = true)
        {
            Stream stream = new MemoryStream(bytes);

            if (compressed)
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            return stream;
        }
    }
}
