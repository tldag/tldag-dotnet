using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class EvaluationFinishedAdapter : BuildAdapter<ProjectEvaluationFinishedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public PropertiesAdapter Globals => new(Args.Properties);
        public PropertiesAdapter Properties => new(Args.Properties);
        public ItemsAdapter Items => new(Args.Items);

        public EvaluationFinishedAdapter(ProjectEvaluationFinishedEventArgs args) : base(args) { }
    }
}
