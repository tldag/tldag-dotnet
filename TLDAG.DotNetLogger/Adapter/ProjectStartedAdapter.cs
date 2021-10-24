using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ProjectStartedAdapter : BuildAdapter<ProjectStartedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }
        public override int PassId => Args.ProjectId;

        private StringDictionaryAdapter? globals = null;
        public IDictionary<string, string> Globals => (globals ??= new(Args.GlobalProperties)).Dictionary;

        public ProjectStartedAdapter(ProjectStartedEventArgs args) : base(args) { }
    }
}
