using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildFinishedAdapter : BuildAdapter
    {
        public BuildFinishedAdapter(BuildFinishedEventArgs args) : base(args) { }
    }
}
