using TLDAG.DotNetLogger.Context;

namespace TLDAG.DotNetLogger.Factory
{
    public partial class DnlFactory
    {
        private DnlContext context = new();

        public void Initialize(DnlContext context) { this.context = context; }
        public void Shutdown() { context = new(); }
    }
}
