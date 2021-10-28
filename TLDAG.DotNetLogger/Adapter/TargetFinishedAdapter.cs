using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TargetFinishedAdapter : BuildAdapter<TargetFinishedEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? base.ProjectFile; }

        public string TargetName { get => Args.TargetName ?? string.Empty; }
        public bool Success { get => Args.Succeeded; }

        public TargetFinishedAdapter(TargetFinishedEventArgs args) : base(args) { }

        public static implicit operator TargetFinishedAdapter(TargetFinishedEventArgs args) => new(args);
    }
}
