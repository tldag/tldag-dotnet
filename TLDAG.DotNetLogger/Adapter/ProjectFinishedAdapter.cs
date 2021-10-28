using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ProjectFinishedAdapter : BuildAdapter<ProjectFinishedEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? base.ProjectFile; }
        public bool Success { get => Args.Succeeded; }

        public ProjectFinishedAdapter(ProjectFinishedEventArgs args) : base(args) { }

        public static implicit operator ProjectFinishedAdapter(ProjectFinishedEventArgs args) => new(args);
    }
}
