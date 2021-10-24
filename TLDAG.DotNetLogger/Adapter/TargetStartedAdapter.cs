using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TargetStartedAdapter : BuildAdapter<TargetStartedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public TargetStartedAdapter(TargetStartedEventArgs args) : base(args) { }
    }
}
