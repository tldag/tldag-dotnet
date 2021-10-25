using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ItemAdapter
    {
        public class EmptyTaskItem : ITaskItem2
        {
            private EmptyTaskItem() { }

            private static EmptyTaskItem instance = new();
            public static ITaskItem2 Instance { get => instance; }

            public string ItemSpec { get => string.Empty; set { } }
            public ICollection MetadataNames { get => Array.Empty<string>(); }
            public int MetadataCount { get => 0; }
            public IDictionary CloneCustomMetadata() => new Dictionary<string, string>();
            public void CopyMetadataTo(ITaskItem destinationItem) { }
            public string GetMetadata(string metadataName) => string.Empty;
            public void RemoveMetadata(string metadataName) { }
            public void SetMetadata(string metadataName, string metadataValue) { }
            public string EvaluatedIncludeEscaped { get => string.Empty; set { } }
            public IDictionary CloneCustomMetadataEscaped() => new Dictionary<string, string>();
            public string GetMetadataValueEscaped(string metadataName) => string.Empty;
            public void SetMetadataValueLiteral(string metadataName, string metadataValue) { }
        }

        private readonly string type;
        private readonly ITaskItem2 item;

        public string Type => type;
        public string Spec => item.EvaluatedIncludeEscaped;

        public ItemAdapter(string type, ITaskItem2? item)
        {
            this.type = type;
            this.item = item ?? EmptyTaskItem.Instance;
        }
    }
}
