using System.IO;
using TLDAG.Core.Model;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.IO
{
    public class TemporaryFile : Resettable
    {
        public FileInfo File => throw NotYetImplemented();
        public string Path => throw NotYetImplemented();

        public TemporaryFile(DirectoryInfo? directory = null)
        {
            throw NotYetImplemented();
        }

        protected override void Reset()
        {
            throw NotYetImplemented();
        }
    }

    public class TemporaryDirectory : Resettable
    {
        public DirectoryInfo Directory => throw NotYetImplemented();
        public string Path => throw NotYetImplemented();

        public TemporaryDirectory(DirectoryInfo? parent = null)
        {
            throw NotYetImplemented();
        }

        protected override void Reset()
        {
            throw NotYetImplemented();
        }
    }

    public class WorkingDirectory : Resettable
    {
        public DirectoryInfo Directory => throw NotYetImplemented();
        public string Path => throw NotYetImplemented();

        public DirectoryInfo Previous => throw NotYetImplemented();

        public WorkingDirectory(DirectoryInfo directory)
        {
            throw NotYetImplemented();
        }

        protected override void Reset()
        {
            throw NotYetImplemented();
        }
    }
}
