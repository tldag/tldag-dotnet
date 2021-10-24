using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildStartedAdapter : BuildAdapter<BuildStartedEventArgs>
    {
        public BuildStartedAdapter(BuildStartedEventArgs args) : base(args) { }
    }
}
