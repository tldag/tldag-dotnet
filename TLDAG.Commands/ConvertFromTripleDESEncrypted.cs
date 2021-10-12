using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Automation;
using TLDAG.Core.Cryptography;
using TLDAG.Core.IO;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.ConvertFrom, "TripleDESEncrypted")]
    [OutputType(typeof(FileInfo))]
    public class ConvertFromTripleDESEncrypted : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 1)]
        public string Password { get; set; } = "";

        [Parameter(Mandatory = false)]
        public string Extension { get; set; } = "";

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            FileInfo inputFile = new(Path);
            string outputName = inputFile.NameWithoutExtension() + Extension;
            FileInfo outputFile = inputFile.GetDirectory().Combine(outputName);

            byte[] encrypted = inputFile.ReadAllBytes();
            byte[] plainBytes = TripleDES.Decrypt(encrypted, Password);

            outputFile.WriteAllBytes(plainBytes);
            WriteObject(new FileInfo(outputFile.FullName));
        }
    }
}
