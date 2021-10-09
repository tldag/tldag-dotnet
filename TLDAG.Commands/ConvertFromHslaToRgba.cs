using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Automation;
using TLDAG.Core.Drawing;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.ConvertFrom, "HslaToRgba")]
    [OutputType(typeof(Rgba))]
    public class ConvertFromHslaToRgba : Command
    {
        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0)]
        public float H = 0.0F;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 1)]
        public float S = 0.0F;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 2)]
        public float L = 0.0F;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 3)]
        public float A = 0.0F;

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            Hsla hsla = new(H, S, L, A);

            WriteObject(hsla.ToRgba());
        }
    }
}
