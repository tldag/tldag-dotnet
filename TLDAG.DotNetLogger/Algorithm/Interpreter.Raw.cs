using Microsoft.Build.Framework;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Algorithm
{
    public static partial class Interpreter
    {
        public static void Transfer(BuildStartedEventArgs args, Builds builds)
            => Transfer(new BuildStartedAdapter(args), builds);

        public static Build Transfer(BuildFinishedEventArgs args, Builds builds)
            => Transfer(new BuildFinishedAdapter(args), builds);

        public static void Transfer(ProjectStartedEventArgs args, Builds builds)
            => Transfer(new ProjectStartedAdapter(args), builds);

        public static void Transfer(ProjectFinishedEventArgs args, Builds builds)
            => Transfer(new ProjectFinishedAdapter(args), builds);

        public static void Transfer(TargetStartedEventArgs args, Builds builds)
            => Transfer(new TargetStartedAdapter(args), builds);

        public static void Transfer(TargetFinishedEventArgs args, Builds builds)
            => Transfer(new TargetFinishedAdapter(args), builds);

        public static void Transfer(TaskStartedEventArgs args, Builds builds)
            => Transfer(new TaskStartedAdapter(args), builds);

        public static void Transfer(TaskFinishedEventArgs args, Builds builds)
            => Transfer(new TaskFinishedAdapter(args), builds);

        public static void Transfer(BuildErrorEventArgs args, Builds builds)
            => Transfer(new BuildErrorAdapter(args), builds);

        public static void Transfer(BuildWarningEventArgs args, Builds builds)
            => Transfer(new BuildWarningAdapter(args), builds);

        public static void Transfer(CustomBuildEventArgs args, Builds builds)
            => Transfer(new CustomEventAdapter(args), builds);

        public static void Transfer(ProjectEvaluationFinishedEventArgs args, Builds builds)
            => Transfer(new EvaluationFinishedAdapter(args), builds);
    }
}
