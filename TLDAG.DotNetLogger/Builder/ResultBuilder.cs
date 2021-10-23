using Microsoft.Build.Framework;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Builder
{
    public class ResultBuilder
    {
        private Result result = new();

        public ResultBuilder Add(BuildStartedEventArgs args) => Add(BuildDataBuilder.Create().Add(args).Build());

        public ResultBuilder Add(ProjectStartedEventArgs args) => Add(ProjectDataBuilder.Create(args).Build());
        public ResultBuilder Add(ProjectFinishedEventArgs args) { return this; }

        public ResultBuilder Add(TargetStartedEventArgs args) { return this; }
        public ResultBuilder Add(TargetFinishedEventArgs args) { return this; }

        public ResultBuilder Add(TaskStartedEventArgs args) { return this; }
        public ResultBuilder Add(TaskFinishedEventArgs args) { return this; }

        public ResultBuilder Add(BuildErrorEventArgs args) { return this; }
        public ResultBuilder Add(BuildWarningEventArgs args) { return this; }

        public ResultBuilder Add(CustomBuildEventArgs args) { return this; }

        public ResultBuilder Add(ProjectEvaluationFinishedEventArgs args) => Add(ProjectDataBuilder.Create(args).Build());

        public ResultBuilder Add(BuildData data)
        {
            result.Environment.AddOrReplace(data.Environment);
            return this;
        }

        public ResultBuilder Add(ProjectData data)
        {
            Project? project = result.GetProject(data.Id, data.File);

            if (project is not null)
            {
                project.AddOrReplace(data.Items);
            }

            return this;
        }

        public Result Build() { Result result = this.result; Clear(); return result; }

        private ResultBuilder Clear() { result = new(); return this; }
    }
}
