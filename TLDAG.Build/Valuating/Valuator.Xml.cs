using System.IO;
using System.Linq;
using System.Text;
using TLDAG.Core;
using TLDAG.Core.Xml;

namespace TLDAG.Build.Valuating
{
    public class XmlValuator : Valuator
    {
        protected XmlValuator() { }

        public override int Valuate(FileInfo file) => file.LoadXmlDocument().AllElements().Count();
        public override int Valuate(FileInfo file, Encoding encoding) => Valuate(file);
        public override int Valuate(string source) => source.ToXmlDocument().AllElements().Count();

        public static XmlValuator Instance { get; } = new();
    }
}
