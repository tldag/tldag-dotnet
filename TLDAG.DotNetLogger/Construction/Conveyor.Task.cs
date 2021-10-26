using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        public static void Transfer(TaskStartedAdapter args, Logs logs)
        {
            Target target = logs.GetTarget(args);

            target.AddTask(args.TaskName, args.TaskId);
        }

        public static void Transfer(TaskFinishedAdapter args, Logs logs)
        {
            BuildTask task = logs.GetTask(args);

            task.Success = args.Success;
        }
    }
}