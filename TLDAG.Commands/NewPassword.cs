using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core.Cryptography;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsCommon.New, "Password")]
    [OutputType(typeof(string))]
    public class NewPassword : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public int Length { get; set; } = 24;

        protected override void ProcessRecord()
        {
            WriteObject(Passwords.NewPassword(Length));
        }
    }
}
