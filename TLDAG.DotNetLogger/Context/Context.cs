using System;
using System.Runtime.InteropServices;
using TLDAG.DotNetLogger.Adapter;
using static System.StringComparer;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlContext
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly StringComparer FileNameComparer = IsWindows ? OrdinalIgnoreCase : Ordinal;

        public DnlConfig Config { get; private set; } = DnlConfig.Invalid;
        public DnlRestrictions Restrictions { get; } = new();
        public DnlElements Elements { get; } = new(FileNameComparer);
        public DnlEvents Events { get; } = new();

        public void Initialize(DnlConfig config)
        {
            Config = config;
            Restrictions.Initialize(Config);
            Elements.Initialize();
            Events.Initialize();
        }

        public void Shutdown()
        {
            Events.Shutdown();
            Elements.Shutdown();
            Restrictions.Shutdown();
            Config = DnlConfig.Invalid;
        }

        public void Add(BuildStartedAdapter args) { Events.Add(args); }
        public void Add(BuildFinishedAdapter args) { Events.Add(args); }

        public void Add(ProjectStartedAdapter args) { Elements.Add(args); Events.Add(args); }
        public void Add(ProjectFinishedAdapter args) { Events.Add(args); }

        public void Add(TargetStartedAdapter args) { Elements.Add(args); Events.Add(args); }
        public void Add(TargetFinishedAdapter args) { Events.Add(args); }

        public void Add(TaskStartedAdapter args) { Elements.Add(args); Events.Add(args); }
        public void Add(TaskFinishedAdapter args) { Events.Add(args); }
    }
}
