using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using TLDAG.DotNetLogger.Context;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.DnlSerialization;

namespace TLDAG.DotNetLogger
{
    public class DnlSender : Logger
    {
        public static string GetDescriptor(string pipeHandle)
            => $"{typeof(DnlSender).FullName},\"{typeof(DnlSender).Assembly.Location}\";{pipeHandle}";

        private readonly DnlContext context = new();
        private readonly DnlHandlers handlers = new();

        public override void Initialize(IEventSource eventSource)
        {
            DnlConfig config = DnlConfig.TryParse(Parameters) ?? DnlConfig.Invalid;

            context.Initialize(config);
            handlers.Initialize(context, eventSource);
            handlers.BuildFinished += OnBuildFinished;
        }

        public override void Shutdown()
        {
            handlers.BuildFinished -= OnBuildFinished;
            handlers.Shutdown();
            context.Shutdown();
        }

        private void OnBuildFinished(DnlHandlers source, DnlHandlersBuildFinishedEventArgs args)
        {
            Send(args.Log, context.Config.PipeHandle);
        }

    }
}
