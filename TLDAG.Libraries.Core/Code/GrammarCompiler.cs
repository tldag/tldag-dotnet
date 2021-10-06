using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TLDAG.Libraries.Core.Code
{
    public partial class GrammarCompiler
    {
        public GrammarCompiler()
        {
            throw new NotImplementedException();
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
            { throw new NotImplementedException(); }
            // { Save(compiler.Compile(text), dest); }

        protected virtual void Save(ParseData data, FileInfo dest)
        {
            throw new NotImplementedException();
        }
    }
}
