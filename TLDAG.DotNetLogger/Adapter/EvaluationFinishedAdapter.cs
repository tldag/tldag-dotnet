using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class EvaluationFinishedAdapter : BuildAdapter<ProjectEvaluationFinishedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public EvaluationFinishedAdapter(ProjectEvaluationFinishedEventArgs args) : base(args) { }
    }
}
