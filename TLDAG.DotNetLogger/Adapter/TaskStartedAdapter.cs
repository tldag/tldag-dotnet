using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TaskStartedAdapter : BuildAdapter<TaskStartedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }
        public string? TaskName { get => Args.TaskName; }

        public TaskStartedAdapter(TaskStartedEventArgs args) : base(args) { }
    }
}
