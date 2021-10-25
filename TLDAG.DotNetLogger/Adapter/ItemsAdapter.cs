using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class ItemsAdapter : IEnumerable<ItemAdapter>
    {
        private readonly IEnumerable<DictionaryEntry> entries;

        public ItemsAdapter(IEnumerable? entries)
        {
            if (entries is IEnumerable<DictionaryEntry> dictEntries)
            {
                this.entries = dictEntries;
            }
            else
            {
                this.entries = Array.Empty<DictionaryEntry>();
            }
        }

        public IEnumerator<ItemAdapter> GetEnumerator() => new ItemsAdapterEnumerator(entries);
        IEnumerator IEnumerable.GetEnumerator() => new ItemsAdapterEnumerator(entries);
    }

    public class ItemsAdapterEnumerator : IEnumerator<ItemAdapter>
    {
        private ItemAdapter? current = null;
        public ItemAdapter Current => current ?? throw new NotSupportedException();
        object IEnumerator.Current => current ?? throw new NotSupportedException();

        private readonly IEnumerable<DictionaryEntry> entries;
        private IEnumerator<DictionaryEntry> enumerator;
        private bool done = false;

        public ItemsAdapterEnumerator(IEnumerable<DictionaryEntry> entries)
        {
            this.entries = entries;
            enumerator = entries.GetEnumerator();
        }

        public void Dispose() { }

        public void Reset()
        {
            current = null;
            enumerator = entries.GetEnumerator();
            done = false;
        }

        public bool MoveNext()
        {
            if (done) return false;

            while (enumerator.MoveNext())
            {
                DictionaryEntry entry = enumerator.Current;

                if (entry.Key is string key && entry.Value is ITaskItem2 item)
                {
                    current = new(key, item);
                    return true;
                }
            }

            done = true;

            return false;
        }
    }
}
