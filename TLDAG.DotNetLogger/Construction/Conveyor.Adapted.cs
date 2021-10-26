using System.Collections;
using System.Collections.Generic;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Construction
{
    public static partial class Conveyor
    {
        private static Project GetProject(this Log log, BuildAdapter args)
            => log.GetProject(args.PassId, args.ProjectFile) ?? new();

        private static Project GetProject(this Logs logs, BuildAdapter args)
            => logs.Current.GetProject(args);

        private static Pass GetPass(this Project project, BuildAdapter args)
            => project.Passes.Contains(args.PassId) ? project.Passes.Get(args.PassId) : new();

        private static Pass GetPass(this Log log, BuildAdapter args)
            => log.GetProject(args).GetPass(args);

        private static Pass GetPass(this Logs logs, BuildAdapter args)
            => logs.Current.GetPass(args);

        private static Target GetTarget(this Pass pass, BuildAdapter args)
            => pass.GetTarget(args.TargetId) ?? new();

        private static Target GetTarget(this Project project, BuildAdapter args)
            => project.GetPass(args).GetTarget(args);

        private static Target GetTarget(this Log log, BuildAdapter args)
            => log.GetProject(args).GetTarget(args);

        private static Target GetTarget(this Logs logs, BuildAdapter args)
            => logs.Current.GetTarget(args);

        private static BuildTask GetTask(this Target target, BuildAdapter args)
            => target.GetTask(args.TaskId) ?? new();

        private static BuildTask GetTask(this Pass pass, BuildAdapter args)
            => pass.GetTarget(args).GetTask(args);

        private static BuildTask GetTask(this Project project, BuildAdapter args)
            => project.GetPass(args).GetTask(args);

        private static BuildTask GetTask(this Log log, BuildAdapter args)
            => log.GetProject(args).GetTask(args);

        private static BuildTask GetTask(this Logs logs, BuildAdapter args)
            => logs.Current.GetTask(args);

        private static StringEntry CreateStringEntry(KeyValuePair<string, string> source)
            => new(source.Key, source.Value);

        private static StringEntry CreateStringEntry(DictionaryEntry source)
        {
            string key = source.Key?.ToString() ?? string.Empty;
            string value = source.Value?.ToString() ?? string.Empty;

            return new(key, value);
        }

        private static Item CreateItem(ItemAdapter source)
        {
            Item item = new(source.Type, source.Spec);

            return item;
        }
    }
}