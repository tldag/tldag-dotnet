using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.Serialization;
using static TLDAG.DotNetLogger.Algorithm.Interpreter;

namespace TLDAG.DotNetLogger
{
    public class Sender : Logger
    {
        public static string GetDescriptor(string pipeHandle)
            => $"{typeof(Sender).FullName},\"{typeof(Sender).Assembly.Location}\";{pipeHandle}";

        public string PipeHandle { get => Parameters; }

        private readonly Builds builds = new();

        public override void Initialize(IEventSource eventSource)
        {
            eventSource.AnyEventRaised += OnAnyEventRaised;

            eventSource.BuildStarted += OnBuildStarted;
            eventSource.BuildFinished += OnBuildFinished;

            eventSource.ProjectStarted += OnProjectStarted;
            eventSource.ProjectFinished += OnProjectFinished;

            eventSource.TargetStarted += OnTargetStarted;
            eventSource.TargetFinished += OnTargetFinished;

            eventSource.TaskStarted += OnTaskStarted;
            eventSource.TaskFinished += OnTaskFinished;

            eventSource.ErrorRaised += OnErrorRaised;
            eventSource.WarningRaised += OnWarningRaised;

            eventSource.CustomEventRaised += OnCustomEventRaised;
        }

        private void OnAnyEventRaised(object sender, BuildEventArgs e)
        {
            if (e is ProjectEvaluationFinishedEventArgs fe)
                OnProjectEvaluationFinished(sender, fe);
        }

        private void OnBuildStarted(object sender, BuildStartedEventArgs e) { Transfer(e, builds); }
        private void OnBuildFinished(object sender, BuildFinishedEventArgs e) { Send(Transfer(e, builds), PipeHandle); }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e) { Transfer(e, builds); }
        private void OnProjectFinished(object sender, ProjectFinishedEventArgs e) { Transfer(e, builds); }

        private void OnTargetStarted(object sender, TargetStartedEventArgs e) { Transfer(e, builds); }
        private void OnTargetFinished(object sender, TargetFinishedEventArgs e) { Transfer(e, builds); }

        private void OnTaskStarted(object sender, TaskStartedEventArgs e) { Transfer(e, builds); }
        private void OnTaskFinished(object sender, TaskFinishedEventArgs e) { Transfer(e, builds); }

        private void OnErrorRaised(object sender, BuildErrorEventArgs e) { Transfer(e, builds); }
        private void OnWarningRaised(object sender, BuildWarningEventArgs e) { Transfer(e, builds); }

        private void OnCustomEventRaised(object sender, CustomBuildEventArgs e) { Transfer(e, builds); }

        private void OnProjectEvaluationFinished(object sender, ProjectEvaluationFinishedEventArgs e)
            { Transfer(e, builds); }
    }
}
