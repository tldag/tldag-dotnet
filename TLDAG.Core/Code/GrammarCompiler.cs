using System.Collections.Generic;
using System.IO;
using System.Text;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Code
{
    public partial class GrammarCompiler
    {
        public GrammarCompiler()
        {
            throw NotYetImplemented();
        }

        public void Compile(FileInfo source, FileInfo dest, Encoding? encoding = null)
        {
            string text = File.ReadAllText(source.FullName, encoding ?? Encoding.UTF8);
            Compile(text, dest);
        }

        public void Compile(IEnumerable<FileInfo> sources, DirectoryInfo output, Encoding? encoding = null)
        {
            foreach (FileInfo source in sources)
            {
                string destName = Path.GetFileNameWithoutExtension(source.Name) + ".gz";
                FileInfo dest = new(Path.Combine(output.FullName, destName));

                Compile(source, dest, encoding);
            }
        }

        public virtual void Compile(string text, FileInfo dest)
            { throw NotYetImplemented(); }
            // { Save(compiler.Compile(text), dest); }

        protected virtual void Save(Parse.IData data, FileInfo dest)
        {
            throw NotYetImplemented();
        }
    }
}
