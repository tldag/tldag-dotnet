using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSender : Logger
    {
        private MSBuildEventSenderPipe? pipe;

        public override void Initialize(IEventSource eventSource)
        {
            Console.WriteLine("MSBuildEventSender.Initialize");
            Console.WriteLine(Parameters);

            pipe = new(Parameters);

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

        public override void Shutdown()
        {
            Console.WriteLine("MSBuildEventSender.Shutdown");

            pipe?.Dispose();

            base.Shutdown();
        }

        protected virtual void OnBuildStarted(object sender, BuildStartedEventArgs e)
        {
            Console.WriteLine("MSBuildEventSender.OnBuildStarted");
        }

        protected virtual void OnBuildFinished(object sender, BuildFinishedEventArgs e)
        {
            Console.WriteLine("MSBuildEventSender.OnBuildFinished");
        }

        protected virtual void OnProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            Console.WriteLine("MSBuildEventSender.OnProjectStarted");
            Console.WriteLine($"  {e.ProjectFile}");
            Console.WriteLine($"  {e.TargetNames}");
        }

        protected virtual void OnProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            Console.WriteLine("MSBuildEventSender.OnProjectFinished");
            Console.WriteLine($"  {e.ProjectFile}");
        }

        protected virtual void OnTargetStarted(object sender, TargetStartedEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnTargetStarted({e.TargetName})");
        }

        protected virtual void OnTargetFinished(object sender, TargetFinishedEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnTargetFinished({e.TargetName})");
        }

        protected virtual void OnTaskStarted(object sender, TaskStartedEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnTaskStarted({e.TaskName})");
        }

        protected virtual void OnTaskFinished(object sender, TaskFinishedEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnTaskFinished({e.TaskName})");
        }

        protected virtual void OnErrorRaised(object sender, BuildErrorEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnErrorRaised({e.Message})");
        }

        protected virtual void OnWarningRaised(object sender, BuildWarningEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnWarningRaised({e.Message})");
        }

        protected virtual void OnCustomEventRaised(object sender, CustomBuildEventArgs e)
        {
            Console.WriteLine($"MSBuildEventSender.OnCustomEventRaised({e.Message})");
        }
    }
}
