using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TLDAG.Core.Internal
{
    internal class StringToLinesEnumerator : IEnumerator<string>
    {
        public string Current => Contract.State.NotNull(current, "");
        object IEnumerator.Current => Contract.State.NotNull(current, "");

        private readonly string text;

        private StringReader reader;
        private string? current = null;
        private bool done = false;

        public StringToLinesEnumerator(string text)
        {
            this.text = text;
            reader = new(text);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            reader.Dispose();
        }

        public void Reset()
        {
            reader.Dispose();
            reader = new(text);
            current = null;
            done = false;
        }

        public bool MoveNext()
        {
            if (done) return false;
            current = reader.ReadLine();
            if (current is null) done = true;
            return current is not null;
        }
    }

    internal class StringToLines : IEnumerable<string>
    {
        private readonly string text;

        public StringToLines(string text) { this.text = text; }

        public IEnumerator<string> GetEnumerator() => new StringToLinesEnumerator(text);
        IEnumerator IEnumerable.GetEnumerator() => new StringToLinesEnumerator(text);
    }
}