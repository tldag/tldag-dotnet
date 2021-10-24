using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class TaskFinishedAdapter : BuildAdapter<TaskFinishedEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public TaskFinishedAdapter(TaskFinishedEventArgs args) : base(args) { }
    }
}
