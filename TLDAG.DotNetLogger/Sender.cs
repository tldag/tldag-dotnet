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

        private readonly Builds _builds = new();

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

        private void OnBuildStarted(object sender, BuildStartedEventArgs e)
            { Transfer(e, _builds); }

        private void OnBuildFinished(object sender, BuildFinishedEventArgs e)
            { Send(Transfer(e, _builds), PipeHandle); }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e) { }
        private void OnProjectFinished(object sender, ProjectFinishedEventArgs e) { }

        private void OnTargetStarted(object sender, TargetStartedEventArgs e) { }
        private void OnTargetFinished(object sender, TargetFinishedEventArgs e) { }

        private void OnTaskStarted(object sender, TaskStartedEventArgs e) { }
        private void OnTaskFinished(object sender, TaskFinishedEventArgs e) { }

        private void OnErrorRaised(object sender, BuildErrorEventArgs e) { }
        private void OnWarningRaised(object sender, BuildWarningEventArgs e) { }

        private void OnCustomEventRaised(object sender, CustomBuildEventArgs e) { }

        private void OnProjectEvaluationFinished(object sender, ProjectEvaluationFinishedEventArgs e) { }
    }
}
