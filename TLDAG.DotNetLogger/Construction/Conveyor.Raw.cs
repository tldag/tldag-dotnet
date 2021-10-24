using Microsoft.Build.Framework;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        public static void Transfer(BuildStartedEventArgs args, Logs logs)
            => Transfer(new BuildStartedAdapter(args), logs);

        public static Log Transfer(BuildFinishedEventArgs args, Logs logs)
            => Transfer(new BuildFinishedAdapter(args), logs);

        public static void Transfer(ProjectStartedEventArgs args, Logs logs)
            => Transfer(new ProjectStartedAdapter(args), logs);

        public static void Transfer(ProjectFinishedEventArgs args, Logs logs)
            => Transfer(new ProjectFinishedAdapter(args), logs);

        public static void Transfer(TargetStartedEventArgs args, Logs logs)
            => Transfer(new TargetStartedAdapter(args), logs);

        public static void Transfer(TargetFinishedEventArgs args, Logs logs)
            => Transfer(new TargetFinishedAdapter(args), logs);

        public static void Transfer(TaskStartedEventArgs args, Logs logs)
            => Transfer(new TaskStartedAdapter(args), logs);

        public static void Transfer(TaskFinishedEventArgs args, Logs logs)
            => Transfer(new TaskFinishedAdapter(args), logs);

        public static void Transfer(BuildErrorEventArgs args, Logs logs)
            => Transfer(new BuildErrorAdapter(args), logs);

        public static void Transfer(BuildWarningEventArgs args, Logs logs)
            => Transfer(new BuildWarningAdapter(args), logs);

        public static void Transfer(CustomBuildEventArgs args, Logs logs)
            => Transfer(new CustomEventAdapter(args), logs);

        public static void Transfer(ProjectEvaluationFinishedEventArgs args, Logs logs)
            => Transfer(new EvaluationFinishedAdapter(args), logs);
    }
}
