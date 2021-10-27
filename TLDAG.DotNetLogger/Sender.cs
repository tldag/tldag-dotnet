using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.Serialization;
using static TLDAG.DotNetLogger.Construction.Conveyor;
using System;
using TLDAG.DotNetLogger.Context;

namespace TLDAG.DotNetLogger
{
    public class DnlSender : Logger
    {
        public static string GetDescriptor(string pipeHandle)
            => $"{typeof(DnlSender).FullName},\"{typeof(DnlSender).Assembly.Location}\";{pipeHandle}";

        public string PipeHandle { get => Parameters; }

        private IEventSource? eventSource = null;
        private readonly Logs logs = new();

        public override void Initialize(IEventSource eventSource)
        {
            Invoke(() =>
            {
                InitializeLogs();
                InitializeHandlers(eventSource);
            });
        }

        public override void Shutdown()
        {
            Invoke(() =>
            {
                ShutdownHandlers();
                ShutdownLogs();
            });
        }

        private void InitializeLogs()
        {
            DnlConfig config = DnlConfig.Parse(Parameters);
            DnlContext context = new(config);
        }

        private void ShutdownLogs() { }

        private void InitializeHandlers(IEventSource eventSource)
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

            this.eventSource = eventSource;
        }

        private void ShutdownHandlers()
        {
            if (eventSource is not null)
            {
                eventSource.AnyEventRaised -= OnAnyEventRaised;

                eventSource.BuildStarted -= OnBuildStarted;
                eventSource.BuildFinished -= OnBuildFinished;

                eventSource.ProjectStarted -= OnProjectStarted;
                eventSource.ProjectFinished -= OnProjectFinished;

                eventSource.TargetStarted -= OnTargetStarted;
                eventSource.TargetFinished -= OnTargetFinished;

                eventSource.TaskStarted -= OnTaskStarted;
                eventSource.TaskFinished -= OnTaskFinished;

                eventSource.ErrorRaised -= OnErrorRaised;
                eventSource.WarningRaised -= OnWarningRaised;

                eventSource.CustomEventRaised -= OnCustomEventRaised;

                eventSource = null;
            }
        }

        private void OnAnyEventRaised(object sender, BuildEventArgs e)
        {
            if (e is ProjectEvaluationFinishedEventArgs fe)
                OnProjectEvaluationFinished(sender, fe);
        }

        private void OnBuildStarted(object sender, BuildStartedEventArgs e) { Transfer(e, logs); }
        private void OnBuildFinished(object sender, BuildFinishedEventArgs e) { Send(Transfer(e, logs), PipeHandle); }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e) { Transfer(e, logs); }
        private void OnProjectFinished(object sender, ProjectFinishedEventArgs e) { Transfer(e, logs); }

        private void OnTargetStarted(object sender, TargetStartedEventArgs e) { Transfer(e, logs); }
        private void OnTargetFinished(object sender, TargetFinishedEventArgs e) { Transfer(e, logs); }

        private void OnTaskStarted(object sender, TaskStartedEventArgs e) { Transfer(e, logs); }
        private void OnTaskFinished(object sender, TaskFinishedEventArgs e) { Transfer(e, logs); }

        private void OnErrorRaised(object sender, BuildErrorEventArgs e) { Transfer(e, logs); }
        private void OnWarningRaised(object sender, BuildWarningEventArgs e) { Transfer(e, logs); }

        private void OnCustomEventRaised(object sender, CustomBuildEventArgs e) { Transfer(e, logs); }

        private void OnProjectEvaluationFinished(object sender, ProjectEvaluationFinishedEventArgs e)
            { Transfer(e, logs); }

        private void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
