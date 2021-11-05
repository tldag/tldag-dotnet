using System.IO;
using System.Text;
using TLDAG.Core.IO;

namespace TLDAG.Build.Valuating
{
    public class Valuator
    {
        public virtual int Valuate(FileInfo file) => Valuate(file.ReadAllText());
        public virtual int Valuate(FileInfo file, Encoding encoding) => Valuate(file.ReadAllText(encoding));
        public virtual int Valuate(string source) => 0;
    }
}
