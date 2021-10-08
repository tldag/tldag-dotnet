using System.Management.Automation;
using TLDAG.Automation;
using TLDAG.Core.Cryptography;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsCommon.New, "Password")]
    [OutputType(typeof(string))]
    public class NewPassword : Command
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public int Length { get; set; } = 24;

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            WriteObject(Passwords.NewPassword(Length));
        }
    }
}
