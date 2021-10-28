using System.Collections.Generic;
using System.Linq;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.Context.DnlContextKeys;

namespace TLDAG.DotNetLogger.Context
{
    public static class DnlFactory
    {
        public static Log CreateLog(DnlContext context)
        {
            Log log = new();

            if (context.Events.RemoveBuild(out BuildStartedAdapter started, out BuildFinishedAdapter finished))
                log.Success = finished.Success;

            log.Projects = CreateProjects(context);

            return log;
        }

        private static List<Project> CreateProjects(DnlContext context)
            => context.Elements.ProjectFiles.Select(file => CreateProject(file, context)).ToList();

        private static Project CreateProject(string file, DnlContext context)
        {
            Project project = new(file);

            project.Passes = CreatePasses(file, context);

            return project;
        }

        private static Passes? CreatePasses(string file, DnlContext context)
        {
            IEnumerable<int> passIds = context.Elements.GetPasses(file);

            return passIds.Any() ? new(passIds.Select(passId => CreatePass(passId, context)).ToList()) : null;
        }

        private static Targets? CreateTargets(int passId, DnlContext context)
        {
            IEnumerable<PassTarget> keys = context.Elements.GetTargets(passId);

            return keys.Any() ? new(keys.Select(key => CreateTarget(key, context)).ToList()) : null;
        }

        private static BuildTasks? CreateTasks(PassTarget passTarget, DnlContext context)
        {
            IEnumerable<TargetTask> keys = context.Elements.GetTasks(passTarget);

            return keys.Any() ? new(keys.Select(key => CreateTask(key, context)).ToList()) : null;
        }

        private static Pass CreatePass(int passId, DnlContext context)
        {
            Pass pass = new(passId);

            if (context.Events.RemovePass(passId, out ProjectStartedAdapter started, out ProjectFinishedAdapter finished))
            {
                pass.Success = finished.Success;
                pass.Targets = CreateTargets(passId, context);
            }

            return pass;
        }

        private static Target CreateTarget(PassTarget key, DnlContext context)
        {
            Target target = new(key.TargetId);

            if (context.Events.RemoveTarget(key, out TargetStartedAdapter started, out TargetFinishedAdapter finished))
            {
                target.Name = started.TargetName;
                target.Tasks = CreateTasks(key, context);
                target.Success = finished.Success;
            }

            return target;
        }

        private static BuildTask CreateTask(TargetTask key, DnlContext context)
        {
            BuildTask task = new(key.TaskId);

            if (context.Events.RemoveTask(key, out TaskStartedAdapter started, out TaskFinishedAdapter finished))
            {
                task.Name = started.TaskName;
                task.Success = finished.Success;
            }

            return task;
        }
    }
}
