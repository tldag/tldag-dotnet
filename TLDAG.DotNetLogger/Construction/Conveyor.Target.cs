using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        public static void Transfer(TargetStartedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            pass.AddTarget(args.TargetName, args.TargetId);
        }

        public static void Transfer(TargetFinishedAdapter args, Logs logs)
        {
            Target target = logs.GetTarget(args);

            target.Success = args.Success;
        }
    }
}