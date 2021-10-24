using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ContextAdapter
    {
        private readonly BuildEventContext context;

        public int ProjectId { get => context.ProjectInstanceId; }

        public ContextAdapter(BuildEventContext? context)
        {
            this.context = context ?? BuildEventContext.Invalid;
        }
    }
}
