using System;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Adapter
{
    public class PropertiesAdapter : IEnumerable<DictionaryEntry>
    {
        private readonly IEnumerable<DictionaryEntry> entries;

        public PropertiesAdapter(IEnumerable? entries)
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

        public IEnumerator<DictionaryEntry> GetEnumerator() => new PropertiesAdapterEnumerator(entries);
        IEnumerator IEnumerable.GetEnumerator() => new PropertiesAdapterEnumerator(entries);
    }

    public class PropertiesAdapterEnumerator : IEnumerator<DictionaryEntry>
    {
        private DictionaryEntry? current = null;
        public DictionaryEntry Current => current ?? throw new NotSupportedException();
        object IEnumerator.Current => current ?? throw new NotSupportedException();

        private readonly IEnumerable<DictionaryEntry> entries;
        private IEnumerator<DictionaryEntry> enumerator;
        private bool done;

        public PropertiesAdapterEnumerator(IEnumerable<DictionaryEntry> entries)
        {
            this.entries = entries;

            enumerator = entries.GetEnumerator();
            done = false;
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

                if (entry.Key is not null && entry.Value is not null)
                {
                    current = entry;
                    return true;
                }
            }

            done = true;

            return false;
        }
    }
}
