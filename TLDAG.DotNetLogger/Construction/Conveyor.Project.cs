using System.Linq;
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
            pass.Properties.AddOrReplace(args.Properties);
            pass.Items.AddOrReplace(args.Items.Select(CreateItem));
        }

        public static void Transfer(EvaluationFinishedAdapter args, Logs logs)
        {
            Project project = logs.GetProject(args);

            project.Globals.AddOrReplace(args.Properties);
            project.Properties.AddOrReplace(args.Properties);
        }

        public static void Transfer(ProjectFinishedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            pass.Success = args.Success;
        }
    }
}