using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.IO;

namespace TLDAG.Libraries.Core.CodeGen.Grammar
{
    public partial class GrammarCompiler
    {
        protected readonly Gramm.Compiler compiler;

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
            { Save(compiler.Compile(text), dest); }

        protected virtual void Save(Parse.Data data, FileInfo dest)
        {
            throw new NotImplementedException();
        }
    }
}
