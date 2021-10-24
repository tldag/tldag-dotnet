using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        private static Project GetProject(this Log log, BuildAdapter args)
            => log.GetProject(args.PassId, args.ProjectFile) ?? new();

        private static Project GetProject(this Logs logs, BuildAdapter args)
            => logs.Current.GetProject(args);

        private static Pass GetPass(this Project project, BuildAdapter args)
            => project.HasPass(args.PassId) ? project.GetPass(args.PassId) : new();

        private static Pass GetPass(this Log log, BuildAdapter args)
            => log.GetProject(args).GetPass(args);

        private static Pass GetPass(this Logs logs, BuildAdapter args)
            => logs.Current.GetPass(args);

        private static Target GetPassTarget(this Pass pass, BuildAdapter args)
            => pass.GetTarget(args.TargetId) ?? new();

        private static Target GetPassTarget(this Project project, BuildAdapter args)
            => project.GetPass(args).GetPassTarget(args);

        private static Target GetPassTarget(this Log log, BuildAdapter args)
            => log.GetProject(args).GetPassTarget(args);

        private static Target GetPassTarget(this Logs logs, BuildAdapter args)
            => logs.Current.GetPassTarget(args);

        private static BuildTask GetTask(this Target target, BuildAdapter args)
            => target.GetTask(args.TaskId) ?? new();

        private static BuildTask GetTask(this Pass pass, BuildAdapter args)
            => pass.GetPassTarget(args).GetTask(args);

        private static BuildTask GetTask(this Project project, BuildAdapter args)
            => project.GetPass(args).GetTask(args);

        private static BuildTask GetTask(this Log log, BuildAdapter args)
            => log.GetProject(args).GetTask(args);

        private static BuildTask GetTask(this Logs logs, BuildAdapter args)
            => logs.Current.GetTask(args);
    }
}