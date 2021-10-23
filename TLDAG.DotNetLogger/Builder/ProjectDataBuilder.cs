using Microsoft.Build.Framework;
using System.Collections;
using System.Collections.Generic;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Builder
{
    public class ProjectDataBuilder
    {
        private readonly int id;
        private readonly string? file;

        private readonly List<ItemData> items = new();

        private ProjectDataBuilder(int id, string? file) { this.id = id; this.file = file; }

        public static ProjectDataBuilder Create(int id, string? file) => new(id, file);

        public static ProjectDataBuilder Create(ProjectStartedEventArgs args)
            => Create(args.ProjectId, args.ProjectFile).Items(args.Items);

        public static ProjectDataBuilder Create(ProjectEvaluationFinishedEventArgs args)
            => Create(-1, args.ProjectFile).Items(args.Items);

        public ProjectDataBuilder Items(IEnumerable? items)
        {
            if (items is not null)
                this.items.AddRange(ItemDataBuilder.Create().Add(items).Build());

            return this;
        }

        public ProjectData Build()
        {
            return new(id, file, items);
        }
    }
}
