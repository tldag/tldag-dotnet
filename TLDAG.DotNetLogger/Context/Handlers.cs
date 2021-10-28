using Microsoft.Build.Framework;
using System;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.Context.DnlFactory;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlHandlersBuildFinishedEventArgs : EventArgs
    {
        public Log Log { get; }
        public DnlHandlersBuildFinishedEventArgs(Log log) { Log = log; }
    }

    public delegate void DnlHandlersBuildFinishedHandler(DnlHandlers source, DnlHandlersBuildFinishedEventArgs args);

    public class DnlHandlers
    {
        public event DnlHandlersBuildFinishedHandler? BuildFinished;

        private DnlContext context = new();
        private IEventSource? eventSource = null;

        public void Initialize(DnlContext context, IEventSource eventSource)
        {
            Shutdown();

            this.context = context;
            this.eventSource = eventSource;

            this.eventSource.BuildStarted += OnBuildStarted;
            this.eventSource.BuildFinished += OnBuildFinished;

            this.eventSource.ProjectStarted += OnProjectStarted;
            this.eventSource.ProjectFinished += OnProjectFinished;

            this.eventSource.TargetStarted += OnTargetStarted;
            this.eventSource.TargetFinished += OnTargetFinished;

            this.eventSource.TaskStarted += OnTaskStarted;
            this.eventSource.TaskFinished += OnTaskFinished;
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

            context = new();
        }

        private void OnBuildStarted(object _, BuildStartedEventArgs e) { Invoke(() => { context.Add(e); }); }

        private void OnBuildFinished(object _, BuildFinishedEventArgs e)
        {
            Invoke(() =>
            {
                context.Add(e);
                Raise(CreateLog(context));
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

        private void Raise(Log log)
        {
            if (BuildFinished is not null)
                BuildFinished.Invoke(this, new(log));
        }
    }
}
