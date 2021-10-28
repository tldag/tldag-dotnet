using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TaskStartedAdapter : BuildAdapter<TaskStartedEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? base.ProjectFile; }
        public string TaskName { get => Args.TaskName ?? string.Empty; }

        public TaskStartedAdapter(TaskStartedEventArgs args) : base(args) { }

        public static implicit operator TaskStartedAdapter(TaskStartedEventArgs args) => new(args);
    }
}
