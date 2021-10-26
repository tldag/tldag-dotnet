using System.Collections.Generic;
using System.Linq;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        private static Dictionary<int, ProjectStartedAdapter> projectAdapters = new();

        public static void Transfer(ProjectStartedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            Transfer(args, pass);

            projectAdapters.Remove(pass.Id);
            projectAdapters[pass.Id] = args;
        }

        public static void Transfer(ProjectFinishedAdapter args, Logs logs)
        {
            Pass pass = logs.GetPass(args);

            pass.Success = args.Success;

            if (projectAdapters.TryGetValue(pass.Id, out ProjectStartedAdapter adpt))
                Transfer(adpt, pass);
        }

        private static void Transfer(ProjectStartedAdapter args, Pass pass)
        {
            pass.SetGlobals(args.Globals.Select(CreateStringEntry));
            pass.SetProperties(args.Properties.Select(CreateStringEntry));
            pass.SetItems(args.Items.Select(CreateItem));
        }

        public static void Transfer(EvaluationFinishedAdapter args, Logs logs)
        {
            Project project = logs.GetProject(args);

            project.SetGlobals(args.Globals.Select(CreateStringEntry));
            project.SetProperties(args.Properties.Select(CreateStringEntry));
            project.SetItems(args.Items.Select(CreateItem));
        }
    }
}