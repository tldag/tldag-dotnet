﻿using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildAdapter
    {
        private readonly BuildEventArgs args;

        private ContextAdapter? context = null;
        private ContextAdapter Context { get => context ??= new(args.BuildEventContext); }

        public virtual string? ProjectFile { get => null; }
        public virtual int PassId { get => Context.ProjectId; }
        public virtual int TargetId { get => Context.TargetId; }
        public virtual int TaskId { get => Context.TaskId; }

        public BuildAdapter(BuildEventArgs args) { this.args = args; }
    }

    public class BuildAdapter<T> : BuildAdapter
        where T : BuildEventArgs
    {
        public T Args { get; }

        public BuildAdapter(T args) : base(args) { Args = args; }
    }
}
