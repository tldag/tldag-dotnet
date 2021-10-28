using Microsoft.Build.Framework;
using System;
using TLDAG.DotNetLogger.Factory;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlHandlersBuildFinishedEventArgs : EventArgs
    {
        public DnlLog Log { get; }
        public DnlHandlersBuildFinishedEventArgs(DnlLog log) { Log = log; }
    }

    public delegate void DnlHandlersBuildFinishedHandler(DnlHandlers source, DnlHandlersBuildFinishedEventArgs args);

    public class DnlHandlers
    {
        public event DnlHandlersBuildFinishedHandler? BuildFinished;

        private DnlContext context = new();
        private DnlFactory factory = new();
        private IEventSource? eventSource = null;

        public void Initialize(DnlContext context, IEventSource eventSource)
        {
            Shutdown();

            this.context = context;
            this.eventSource = eventSource;

            factory.Initialize(context);

            eventSource.BuildStarted += OnBuildStarted;
            eventSource.BuildFinished += OnBuildFinished;

            eventSource.ProjectStarted += OnProjectStarted;
            eventSource.ProjectFinished += OnProjectFinished;

            eventSource.TargetStarted += OnTargetStarted;
            eventSource.TargetFinished += OnTargetFinished;

            eventSource.TaskStarted += OnTaskStarted;
            eventSource.TaskFinished += OnTaskFinished;
        }

        public void Shutdown()
        {
            if (eventSource is not null)
            {
                eventSource.BuildStarted -= OnBuildStarted;
                eventSource.BuildFinished -= OnBuildFinished;

                eventSource.ProjectStarted -= OnProjectStarted;
                eventSource.ProjectFinished -= OnProjectFinished;

                eventSource.TargetStarted -= OnTargetStarted;
                eventSource.TargetFinished -= OnTargetFinished;

                eventSource.TaskStarted -= OnTaskStarted;
                eventSource.TaskFinished -= OnTaskFinished;

                eventSource = null;
            }

            factory.Shutdown();
            context = new();
        }

        private void OnBuildStarted(object _, BuildStartedEventArgs e) { Invoke(() => { context.Add(e); }); }

        private void OnBuildFinished(object _, BuildFinishedEventArgs e)
        {
            Invoke(() =>
            {
                context.Add(e);
                Raise(factory.CreateLog());
            });
        }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e) { Invoke(() => { context.Add(e); }); }
        private void OnProjectFinished(object _, ProjectFinishedEventArgs e) { Invoke(() => { context.Add(e); }); }

        private void OnTargetStarted(object sender, TargetStartedEventArgs e) { Invoke(() => { context.Add(e); }); }
        private void OnTargetFinished(object sender, TargetFinishedEventArgs e) { Invoke(() => { context.Add(e); }); }

        private void OnTaskStarted(object sender, TaskStartedEventArgs e) { Invoke(() => { context.Add(e); }); }
        private void OnTaskFinished(object sender, TaskFinishedEventArgs e) { Invoke(() => { context.Add(e); }); }

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

        private void Raise(DnlLog log)
        {
            if (BuildFinished is not null)
                BuildFinished.Invoke(this, new(log));
        }
    }
}
