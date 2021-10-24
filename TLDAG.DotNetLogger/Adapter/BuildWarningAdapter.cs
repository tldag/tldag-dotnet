using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildWarningAdapter : BuildAdapter<BuildWarningEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public BuildWarningAdapter(BuildWarningEventArgs args) : base(args) { }
    }
}
