using System.Collections.Generic;
using TLDAG.DotNetLogger.Adapter;
using static TLDAG.DotNetLogger.Context.DnlContextKeys;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlEvents
    {
        private BuildStartedAdapter? buildStarted = null;
        private BuildFinishedAdapter? buildFinished = null;

        private readonly Dictionary<int, ProjectStartedAdapter> passesStarted = new();
        private readonly Dictionary<int, ProjectFinishedAdapter> passesFinished = new();

        private readonly Dictionary<PassTarget, TargetStartedAdapter> targetsStarted = new();
        private readonly Dictionary<PassTarget, TargetFinishedAdapter> targetsFinished = new();

        private readonly Dictionary<TargetTask, TaskStartedAdapter> tasksStarted = new();
        private readonly Dictionary<TargetTask, TaskFinishedAdapter> tasksFinished = new();

        public void Initialize()
        {
            Shutdown();
        }

        public void Shutdown()
        {
            tasksFinished.Clear();
            tasksStarted.Clear();
            targetsFinished.Clear();
            targetsStarted.Clear();
            passesFinished.Clear();
            passesStarted.Clear();
            buildStarted = null;
            buildFinished = null;
        }

        public void Add(BuildStartedAdapter started) { buildStarted = started; }
        public void Add(BuildFinishedAdapter finished) { buildFinished = finished; }

        public void Add(ProjectStartedAdapter started)
        {
            int passId = started.PassId;

            if (passId >= 0)
            {
                passesStarted.Remove(passId);
                passesStarted[passId] = started;
            }
        }

        public void Add(ProjectFinishedAdapter finished)
        {
            int passId = finished.PassId;

            if (passId >= 0)
            {
                passesFinished.Remove(passId);
                passesFinished[passId] = finished;
            }
        }

        public void Add(TargetStartedAdapter started)
        {
            PassTarget key = new(started);

            if (key.IsValid)
            {
                targetsStarted.Remove(key);
                targetsStarted[key] = started;
            }
        }

        public void Add(TargetFinishedAdapter finished)
        {
            PassTarget key = new(finished);

            if (key.IsValid)
            {
                targetsFinished.Remove(key);
                targetsFinished[key] = finished;
            }
        }

        public void Add(TaskStartedAdapter started)
        {
            TargetTask key = new(started);

            if (key.IsValid)
            {
                tasksStarted.Remove(key);
                tasksStarted[key] = started;
            }
        }

        public void Add(TaskFinishedAdapter finished)
        {
            TargetTask key = new(finished);

            if (key.IsValid)
            {
                tasksFinished.Remove(key);
                tasksFinished[key] = finished;
            }
        }

        public bool RemoveBuild(out BuildStartedAdapter started, out BuildFinishedAdapter finished)
        {
            bool result = buildStarted is not null && buildFinished is not null;

            started = buildStarted ?? new();
            finished = buildFinished ?? new();

            buildStarted = null; buildFinished = null;

            return result;
        }

        public bool RemovePass(int passId, out ProjectStartedAdapter? started, out ProjectFinishedAdapter? finished)
        {
            bool hasStarted = passesStarted.TryGetValue(passId, out started);
            bool hasFinished = passesFinished.TryGetValue(passId, out finished);

            passesStarted.Remove(passId);
            passesFinished.Remove(passId);

            return hasStarted && hasFinished;
        }

        public bool RemoveTarget(PassTarget key, out TargetStartedAdapter? started, out TargetFinishedAdapter? finished)
        {
            bool hasStarted = targetsStarted.TryGetValue(key, out started);
            bool hasFinished = targetsFinished.TryGetValue(key, out finished);

            targetsStarted.Remove(key);
            targetsFinished.Remove(key);

            return hasStarted && hasFinished;
        }

        public bool RemoveTask(TargetTask key, out TaskStartedAdapter? started, out TaskFinishedAdapter? finished)
        {
            bool hasStarted = tasksStarted.TryGetValue(key, out started);
            bool hasFinished = tasksFinished.TryGetValue(key, out finished);

            tasksStarted.Remove(key);
            tasksFinished.Remove(key);

            return hasStarted && hasFinished;
        }
    }
}
