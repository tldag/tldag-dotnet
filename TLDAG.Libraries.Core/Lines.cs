using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TLDAG.Libraries.Core
{
    public abstract class Lines : IEnumerable<string>
    {
        public abstract class LinesEnumerator : IEnumerator<string>
        {
            protected string? current;
            public string Current => current ?? throw new InvalidOperationException();

            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public virtual void Dispose() { GC.SuppressFinalize(this); }

            public abstract bool MoveNext();

            public virtual void Reset()
                { current = null; }
        }

        public IEnumerator<string> GetEnumerator()
            => CreateEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => CreateEnumerator();

        protected abstract IEnumerator<string> CreateEnumerator();
    }

    public class StringLines : Lines
    {
        public string Source { get; }

        public StringLines(string source)
        {
            Source = source;
        }

        public static StringLines Create(string source) => new(source);

        protected override IEnumerator<string> CreateEnumerator()
            => new StringLinesEnumerator(Source);

        public class StringLinesEnumerator : LinesEnumerator
        {
            protected readonly string source;
            protected StringReader reader;

            public StringLinesEnumerator(string source)
            {
                this.source = source;
                reader = new(source);
            }

            public override bool MoveNext()
            {
                current = reader.ReadLine();

                return current != null;
            }

            public override void Reset()
            {
                reader = new(source);

                base.Reset();
            }
        }
    }
}
