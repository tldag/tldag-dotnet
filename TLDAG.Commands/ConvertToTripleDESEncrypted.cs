using System.IO;
using System.Management.Automation;
using TLDAG.Automation;
using TLDAG.Core.Cryptography;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.ConvertTo, "TripleDESEncrypted")]
    [OutputType(typeof(FileInfo))]
    public class ConvertToTripleDESEncrypted : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 1)]
        public string Password { get; set; } = "";

        [Parameter(Mandatory = false)]
        public string Extension { get; set; } = ".enc";

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            byte[] plainBytes = File.ReadAllBytes(Path);
            byte[] encrypted = TripleDES.Encrypt(plainBytes, Password);
            string outputPath = Path + Extension;

            File.WriteAllBytes(outputPath, encrypted);
            WriteObject(new FileInfo(outputPath));
        }
    }
}
