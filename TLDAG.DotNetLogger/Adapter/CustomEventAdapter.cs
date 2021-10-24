using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class CustomEventAdapter : BuildAdapter<CustomBuildEventArgs>
    {
        public CustomEventAdapter(CustomBuildEventArgs args) : base(args) { }
    }
}
