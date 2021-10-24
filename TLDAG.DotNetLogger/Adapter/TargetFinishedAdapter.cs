using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TargetFinishedAdapter : BuildAdapter<TargetFinishedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public string? TargetName { get => Args.TargetName; }
        public bool Success { get => Args.Succeeded; }

        public TargetFinishedAdapter(TargetFinishedEventArgs args) : base(args) { }
    }
}
