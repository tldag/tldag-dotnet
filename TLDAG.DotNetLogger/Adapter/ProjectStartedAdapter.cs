using Microsoft.Build.Framework;
using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ProjectStartedAdapter : BuildAdapter<ProjectStartedEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? ""; }
        public override int PassId => Args.ProjectId;

        public IDictionary<string, string> Globals => new StringDictionaryAdapter(Args.GlobalProperties).Dictionary;
        public PropertiesAdapter Properties => new(Args.Properties);
        public ItemsAdapter Items => new(Args.Items);

        public ProjectStartedAdapter(ProjectStartedEventArgs args) : base(args) { }

        public static implicit operator ProjectStartedAdapter(ProjectStartedEventArgs args) => new(args);
    }
}
