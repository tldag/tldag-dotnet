using System;
using System.Collections.Generic;
using TLDAG.DotNetLogger.Adapter;
using static TLDAG.DotNetLogger.Context.DnlContextKeys;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlElements
    {
        private readonly SortedSet<string> projectFiles;
        private readonly Dictionary<string, SortedSet<int>> projectPasses;
        private readonly Dictionary<int, List<PassTarget>> passTargets;
        private readonly Dictionary<PassTarget, List<TargetTask>> targetTasks;

        public IEnumerable<string> ProjectFiles { get => projectFiles; }

        public DnlElements(StringComparer fileNameComparer)
        {
            projectFiles = new(fileNameComparer);
            projectPasses = new(fileNameComparer);
            passTargets = new();
            targetTasks = new();
        }

        public void Initialize() { Shutdown(); }

        public void Shutdown()
        {
            targetTasks.Clear();
            passTargets.Clear();
            projectPasses.Clear();
            projectFiles.Clear();
        }

        public IEnumerable<int> GetPasses(string projectFile)
        {
            if (projectPasses.TryGetValue(projectFile, out SortedSet<int> passes))
                return passes;

            return Array.Empty<int>();
        }

        public IEnumerable<PassTarget> GetTargets(int passId)
        {
            if (passTargets.TryGetValue(passId, out List<PassTarget> targets))
                return targets;

            return Array.Empty<PassTarget>();
        }

        public IEnumerable<TargetTask> GetTasks(PassTarget targetKey)
        {
            if (targetTasks.TryGetValue(targetKey, out List<TargetTask> tasks))
                return tasks;

            return Array.Empty<TargetTask>();
        }

        public void Add(ProjectStartedAdapter args)
        {
            string projectFile = args.ProjectFile;
            int passId = args.PassId;

            projectFiles.Add(projectFile);

            if (!projectPasses.ContainsKey(projectFile))
                projectPasses[projectFile] = new();

            if (passId >= 0)
                projectPasses[projectFile].Add(passId);
        }

        public void Add(TargetStartedAdapter args)
        {
            PassTarget key = new(args);

            if (!key.IsValid)
                return;

            if (!passTargets.ContainsKey(key.PassId))
                passTargets[key.PassId] = new();

            passTargets[key.PassId].Add(key);
        }

        public void Add(TaskStartedAdapter args)
        {
            PassTarget targetKey = new(args);
            TargetTask taskKey = new(args);

            if (!taskKey.IsValid)
                return;

            if (!targetTasks.ContainsKey(targetKey))
                targetTasks[targetKey] = new();

            targetTasks[targetKey].Add(taskKey);
        }
    }
}
