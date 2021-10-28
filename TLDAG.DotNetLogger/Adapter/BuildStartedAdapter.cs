using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildStartedAdapter : BuildAdapter<BuildStartedEventArgs>
    {
        public BuildStartedAdapter(BuildStartedEventArgs args) : base(args) { }
        public BuildStartedAdapter() : this(new(string.Empty, string.Empty)) { }

        public static implicit operator BuildStartedAdapter(BuildStartedEventArgs args) => new(args);
    }
}
