using System.Collections;
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

            if (context.Events.RemoveBuild(out BuildStartedAdapter started, out BuildFinishedAdapter finished))
                log.Success = finished.Success;

            log.Projects = CreateProjects();

            return log;
        }

        public List<DnlProject> CreateProjects()
            => context.Elements.ProjectFiles.Select(CreateProject).ToList();

        public DnlProject CreateProject(string file)
        {
            DnlProject project = new(file);

            project.Passes = CreatePasses(file);

            return project;
        }

        public List<DnlPass> CreatePasses(string file)
            => context.Elements.GetPasses(file).Select(CreatePass).ToList();

        public DnlPass CreatePass(int passId)
        {
            DnlPass pass = new(passId);

            if (context.Events.RemovePass(passId, out ProjectStartedAdapter started, out ProjectFinishedAdapter finished))
            {
                pass.Success = finished.Success;
                pass.Targets = CreateTargets(passId);
            }

            return pass;
        }

        public List<DnlTarget> CreateTargets(int passId)
            => context.Elements.GetTargets(passId).Select(CreateTarget).ToList();

        public DnlTarget CreateTarget(PassTarget key)
        {
            DnlTarget target = new(key.TargetId);

            if (context.Events.RemoveTarget(key, out TargetStartedAdapter started, out TargetFinishedAdapter finished))
            {
                target.Name = started.TargetName;
                target.Tasks = CreateTasks(key);
                target.Success = finished.Success;
            }

            return target;
        }

        public DnlTask CreateTask(TargetTask key)
        {
            DnlTask task = new(key.TaskId);

            if (context.Events.RemoveTask(key, out TaskStartedAdapter started, out TaskFinishedAdapter finished))
            {
                task.Name = started.TaskName;
                task.Success = finished.Success;
            }

            return task;
        }

        public List<DnlTask> CreateTasks(PassTarget passTarget)
            => context.Elements.GetTasks(passTarget).Select(CreateTask).ToList();
    }
}