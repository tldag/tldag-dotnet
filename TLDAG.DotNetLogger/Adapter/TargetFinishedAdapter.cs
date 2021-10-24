using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TargetFinishedAdapter : BuildAdapter<TargetFinishedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public TargetFinishedAdapter(TargetFinishedEventArgs args) : base(args) { }
    }
}
