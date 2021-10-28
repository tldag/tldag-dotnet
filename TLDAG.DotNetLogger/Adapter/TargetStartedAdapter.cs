using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TargetStartedAdapter : BuildAdapter<TargetStartedEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? base.ProjectFile; }

        public string TargetName { get => Args.TargetName ?? string.Empty; }

        public TargetStartedAdapter(TargetStartedEventArgs args) : base(args) { }

        public static implicit operator TargetStartedAdapter(TargetStartedEventArgs args) => new(args);
    }
}
