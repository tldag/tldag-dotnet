using System.IO;
using System.Management.Automation;
using TLDAG.Automation;
using TLDAG.Core.Cryptography;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsCommon.New, "SnkFile")]
    [OutputType(typeof(FileInfo))]
    public class NewSnkFile : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            File.WriteAllBytes(Path, Keys.NewRsaKeyPair(1024));
            WriteObject(new FileInfo(Path));
        }
    }
}
