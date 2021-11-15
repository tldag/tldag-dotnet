using System;
using TLDAG.DotNetLogger.Adapter;

namespace TLDAG.DotNetLogger.Context
{
    public static class DnlContextKeys
    {
        public class PassTarget : IEquatable<PassTarget>, IComparable<PassTarget>
        {
            public int PassId { get; }
            public int TargetId { get; }

            public virtual bool IsValid { get => PassId >= 0 && TargetId >= 0; }

            public PassTarget(int passId, int targetId) { PassId = passId; TargetId = targetId; }
            public PassTarget(BuildAdapter args) : this(args.PassId, args.TargetId) { }

            public override int GetHashCode() => PassId.GetHashCode() << 9 + TargetId.GetHashCode();
            public override bool Equals(object? obj) => EqualsTo(obj as PassTarget);
            public bool Equals(PassTarget? other) => EqualsTo(other);

            protected bool EqualsTo(PassTarget? other)
            {
                if (other is null) return false;
                return PassId == other.PassId && TargetId == other.TargetId;
            }

            public int CompareTo(PassTarget? other)
            {
                int result = PassId.CompareTo(other?.PassId);

                if (result == 0)
                    result = TargetId.CompareTo(other?.TargetId);

                return result;
            }
        }

        public class TargetTask : PassTarget, IEquatable<TargetTask>, IComparable<TargetTask>
        {
            public int TaskId { get; }
            public override bool IsValid { get => base.IsValid && TaskId >= 0; }

            public TargetTask(int passId, int targetId, int taskId) : base(passId, targetId) { TaskId = taskId; }
            public TargetTask(BuildAdapter args) : this(args.PassId, args.TargetId, args.TaskId) { }

            public override int GetHashCode() => base.GetHashCode() << 9 + TaskId.GetHashCode();
            public override bool Equals(object? obj) => EqualsTo(obj as TargetTask);
            public bool Equals(TargetTask? other) => EqualsTo(other);

            protected bool EqualsTo(TargetTask? other)
            {
                if (other is null) return false;
                return base.EqualsTo(other) && TaskId == other.TaskId;
            }

            public int CompareTo(TargetTask? other)
            {
                int result = base.CompareTo(other);

                if (result == 0)
                    result = TaskId.CompareTo(other?.TaskId);

                return result;
            }
        }
    }
}
