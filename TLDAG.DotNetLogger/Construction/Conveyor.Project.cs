using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        public static void Transfer(ProjectStartedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            pass.Globals.AddOrReplace(args.Globals);
        }

        public static void Transfer(EvaluationFinishedAdapter args, Logs logs)
        {
        }

        public static void Transfer(ProjectFinishedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            pass.Success = args.Success;
        }
    }
}