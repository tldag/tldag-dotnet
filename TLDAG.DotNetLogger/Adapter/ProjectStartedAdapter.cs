using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ProjectStartedAdapter : BuildAdapter<ProjectStartedEventArgs>
    {
        public int Id { get => Args.ProjectId; }
        public override string? ProjectFile { get => Args.ProjectFile; }

        public ProjectStartedAdapter(ProjectStartedEventArgs args) : base(args) { }
    }
}
