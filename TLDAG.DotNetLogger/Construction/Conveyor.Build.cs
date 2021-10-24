using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        public static void Transfer(BuildStartedAdapter args, Logs logs)
        {
            logs.Push();
        }

        public static Log Transfer(BuildFinishedAdapter args, Logs logs)
        {
            Log log = logs.Pop();

            log.Success = args.Success;

            return log;
        }
    }
}