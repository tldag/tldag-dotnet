using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.IO
{
    public static partial class Files
    {
        public static string NextBackupFileName(this FileInfo file)
        {
            throw NotYetImplemented();
        }

        public static FileInfo NextBackupFile(this FileInfo file)
        {
            throw NotYetImplemented();
        }

        public static FileInfo? Backup(this FileInfo file)
        {
            throw NotYetImplemented();
        }
    }
}
