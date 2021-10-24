using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Algorithm
{
    public static partial class Interpreter
    {
        public static void Transfer(BuildStartedAdapter args, Builds builds)
        {
            Build build = builds.StartBuild();

            Transfer(args, build);
        }

        public static Build Transfer(BuildFinishedAdapter args, Builds builds)
        {
            Build build = builds.EndBuild();

            Transfer(args, build);
            build.Success = args.Success;

            return build;
        }

        public static void Transfer(ProjectStartedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.Id, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(ProjectFinishedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(TargetStartedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(TargetFinishedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(TaskStartedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(TaskFinishedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(BuildErrorAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(BuildWarningAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, args.ProjectFile);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(CustomEventAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, null);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        public static void Transfer(EvaluationFinishedAdapter args, Builds builds)
        {
            Build build = builds.GetBuild();
            Project? project = build.GetProject(args.PassId, null);

            if (project is not null)
            {
            }

            Transfer(args, build);
        }

        private static void Transfer(BuildAdapter args, Build build)
        {
        }

    }
}