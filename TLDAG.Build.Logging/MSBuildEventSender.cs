using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using static TLDAG.Build.Logging.MSBuildEventModel;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSender : Logger
    {
        private AnonymousPipeClientStream? pipe;
        private BuildResult result = new();

        private string Handle { get => Parameters.Split(';')[0]; }

        public override void Initialize(IEventSource eventSource)
        {
            pipe = new(PipeDirection.Out, Handle);

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
            pipe = null;

            base.Shutdown();
        }

        protected virtual void OnAnyEventRaised(object sender, BuildEventArgs e)
        {
            if (e is ProjectEvaluationFinishedEventArgs fe)
                OnProjectEvaluationFinished(sender, fe);
        }

        protected virtual void OnBuildStarted(object sender, BuildStartedEventArgs e)
        {
            result.Clear();
            result.Environment.AddEntries(e.BuildEnvironment);
        }

        protected virtual void OnBuildFinished(object sender, BuildFinishedEventArgs e)
        {
            if (pipe is null) return;

            MSBuildEventStream stream = new(pipe);

            stream.Write(JsonConvert.SerializeObject(result));
        }

        protected virtual void OnProjectStarted(object sender, ProjectStartedEventArgs e)
            { OnProjectData(e.ProjectFile, e.GlobalProperties, e.Properties, e.Items); }

        protected virtual void OnProjectEvaluationFinished(object sender, ProjectEvaluationFinishedEventArgs e)
            { OnProjectData(e.ProjectFile, e.GlobalProperties, e.Properties, e.Items); }

        protected virtual void OnProjectData(string file, IEnumerable? globals, IEnumerable? properties, IEnumerable? items)
        {
            Project project = result.GetProject(file);

            if (globals is IDictionary<string, string> globalsDict)
                project.AddGlobalProperties(globalsDict);

            if (globals is IEnumerable<DictionaryEntry> globalsEnum)
                project.AddGlobalProperties(globalsEnum);

            if (properties is IEnumerable<DictionaryEntry> propertiesEnum)
                project.AddProperties(propertiesEnum);

            if (items is IEnumerable<DictionaryEntry> itemsEnum)
                project.AddItems(itemsEnum);
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
