using System.Collections.Generic;
using System.Linq;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.Context.DnlContextKeys;

namespace TLDAG.DotNetLogger.Factory
{
    public partial class DnlFactory
    {
        public DnlLog CreateLog()
        {
            DnlLog log = new();

            if (context.Events.RemoveBuild(out BuildStartedAdapter _, out BuildFinishedAdapter finished))
                log.Success = finished.Success;

            log.Projects = CreateProjects();

            return log;
        }

        public DnlProjects? CreateProjects()
        {
            IEnumerable<string> files = context.Elements.ProjectFiles;

            return files.Any() ? new(files.Select(CreateProject).ToList()) : null;
        }

        public DnlProject CreateProject(string file)
        {
            DnlProject project = new(file);

            project.Passes = CreatePasses(file);

            return project;
        }

        public DnlPasses? CreatePasses(string file)
        {
            IEnumerable<int> passIds = context.Elements.GetPasses(file);

            return passIds.Any() ? new(passIds.Select(CreatePass).ToList()) : null;
        }

        public DnlPass CreatePass(int passId)
        {
            DnlPass pass = new(passId);

            if (context.Events.RemovePass(passId, out ProjectStartedAdapter? started, out ProjectFinishedAdapter? finished))
            {
                pass.Success = finished?.Success ?? false;
                pass.Targets = CreateTargets(passId);
                pass.Items = CreateItems(started?.Items);
            }

            return pass;
        }

        public DnlTargets? CreateTargets(int passId)
        {
            IEnumerable<PassTarget> keys = context.Elements.GetTargets(passId);

            return keys.Any() ? new(keys.Select(CreateTarget).ToList()) : null;
        }

        public DnlTarget CreateTarget(PassTarget key)
        {
            DnlTarget target = new(key.TargetId);

            if (context.Events.RemoveTarget(key, out TargetStartedAdapter? started, out TargetFinishedAdapter? finished))
            {
                target.Name = started?.TargetName ?? string.Empty;
                target.Tasks = CreateTasks(key);
                target.Success = finished?.Success ?? false;
            }

            return target;
        }

        public DnlTask CreateTask(TargetTask key)
        {
            DnlTask task = new(key.TaskId);

            if (context.Events.RemoveTask(key, out TaskStartedAdapter? started, out TaskFinishedAdapter? finished))
            {
                task.Name = started?.TaskName ?? string.Empty;
                task.Success = finished?.Success ?? false;
            }

            return task;
        }

        public List<DnlTask> CreateTasks(PassTarget passTarget)
            => context.Elements.GetTasks(passTarget).Select(CreateTask).ToList();
    }
}