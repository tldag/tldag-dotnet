using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildAdapter
    {
        public int Submission { get; }
        public int Evaluation { get; }

        public BuildAdapter(BuildEventArgs args)
        {
            BuildEventContext context = args.BuildEventContext ?? BuildEventContext.Invalid;

            Evaluation = context.EvaluationId;
            Submission = context.SubmissionId;
        }
    }
}
