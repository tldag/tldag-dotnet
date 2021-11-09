using System.IO;
using System.Management.Automation.Language;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Build.PS
{
    public partial class PSManifest
    {
        public static PSManifest Load(FileInfo file) => Parse(file.ReadAllText());

        public static PSManifest Parse(string text)
        {
            ScriptBlockAst ast = Parser.ParseInput(text, out Token[] tokens, out ParseError[] errors);

            if (errors.Length > 0) throw InvalidArgument(nameof(text), errors[0].Message);

            Ast? data = ast.Find(a => a is HashtableAst, false);

            if (data == null) throw InvalidArgument(nameof(text), "No data");

            return Parse(data.SafeGetValue());
        }

        private static PSManifest Parse(object value)
        {
            throw NotYetImplemented();
        }
    }
}
