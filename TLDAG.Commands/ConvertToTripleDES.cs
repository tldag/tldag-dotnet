using System.Management.Automation;
using TLDAG.Automation;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.ConvertTo, "TripleDES")]
    public class ConvertToTripleDES : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 1)]
        public string Password { get; set; } = "";

        [Parameter(Mandatory = false, Position = 2)]
        public string Output { get; set; } = "";

        [Parameter(Mandatory = false)]
        public string Extension { get; set; } = ".enc";

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            throw NotYetImplemented(nameof(ProcessRecord));
        }
    }
}
