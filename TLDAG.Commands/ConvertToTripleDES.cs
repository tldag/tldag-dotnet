using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.ConvertTo, "TripleDES")]
    public class ConvertToTripleDES : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 1)]
        public string Password { get; set; } = "";

        [Parameter(Mandatory = false, Position = 2)]
        public string Output { get; set; } = "";

        [Parameter(Mandatory = false)]
        public string Extension { get; set; } = ".enc";

        protected override void ProcessRecord()
        {
            throw NotYetImplemented(nameof(ProcessRecord));

            // base.ProcessRecord();
        }
    }
}
