using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using static TLDAG.Build.Logging.MSBuildEventModel;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSender : Logger
    {
        private MSBuildEventSenderPipe? pipe;

        private BuildData build = new();

        public override void Initialize(IEventSource eventSource)
        {
            pipe = CreatePipe();

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

        public override void Shutdown()
        {
            pipe?.Dispose();

            base.Shutdown();
        }

        protected virtual MSBuildEventSenderPipe CreatePipe() => new(Parameters, CreateSerializer());
        protected virtual MSBuildEventSerializer CreateSerializer() => new();

        protected virtual void OnAnyEventRaised(object sender, BuildEventArgs e)
        {
            if (e is ProjectEvaluationFinishedEventArgs fe)
                OnProjectEvaluationFinished(sender, fe);
        }

        protected virtual void OnBuildStarted(object sender, BuildStartedEventArgs e)
        {
            build.Clear();
            build.SetEnvironment(e.BuildEnvironment);
        }

        protected virtual void OnBuildFinished(object sender, BuildFinishedEventArgs e) { pipe?.Send(build); }

        protected virtual void OnProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            Project project = build.GetProject(e.ProjectFile);

            project.AddGlobalProperties(e.GlobalProperties);

            if (e.Properties is IEnumerable<DictionaryEntry> properties)
                project.AddProperties(properties);

            if (e.Items is IEnumerable<DictionaryEntry> items)
                project.AddItems(items);
        }

        protected virtual void OnProjectEvaluationFinished(object sender, ProjectEvaluationFinishedEventArgs e)
        {
            Project project = build.GetProject(e.ProjectFile);

            Console.WriteLine("OnProjectEvaluationFinished");

            if (e.GlobalProperties is IEnumerable<DictionaryEntry> globalProperties)
                project.AddGlobalProperties(globalProperties);

            if (e.Properties is IEnumerable<DictionaryEntry> properties)
                project.AddProperties(properties);

            if (e.Items is IEnumerable<DictionaryEntry> items)
                project.AddItems(items);
        }

        protected virtual void OnProjectFinished(object sender, ProjectFinishedEventArgs e) { }
        protected virtual void OnTargetStarted(object sender, TargetStartedEventArgs e) { }
        protected virtual void OnTargetFinished(object sender, TargetFinishedEventArgs e) { }
        protected virtual void OnTaskStarted(object sender, TaskStartedEventArgs e) { }
        protected virtual void OnTaskFinished(object sender, TaskFinishedEventArgs e) { }
        protected virtual void OnErrorRaised(object sender, BuildErrorEventArgs e) { }
        protected virtual void OnWarningRaised(object sender, BuildWarningEventArgs e) { }
        protected virtual void OnCustomEventRaised(object sender, CustomBuildEventArgs e) { }
    }
}
