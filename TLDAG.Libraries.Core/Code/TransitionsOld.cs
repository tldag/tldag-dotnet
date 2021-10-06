using System.Collections.Generic;
using System.IO;
using TLDAG.Libraries.Core.IO;
using TLDAG.Libraries.Core.Resources;

namespace TLDAG.Libraries.Core.Code
{
    public class TransitionsOld
    {
        private readonly int width;
        private readonly int[][] transitions;

        internal TransitionsOld(int width, int[][] transitions)
        {
            this.width = width;
            this.transitions = transitions;
        }

        public int this[int state, int symbol]
        {
            get => transitions[state][symbol];
        }

        public static TransitionsOld Load(Stream stream)
        {
            IntStreamOld input = new(stream);
            int count = input.Read();
            int width = input.Read();
            int[][] transitions = new int[count][];

            for (int i = 0; i < count; ++i)
            {
                transitions[i] = new int[width];
                input.Read(transitions[i]);
            }

            return new(width, transitions);
        }

        public void Save(Stream stream)
        {
            int count = transitions.Length;
            IntStreamOld output = new(stream);

            output.Write(count);
            output.Write(width);

            for (int i = 0; i < count; ++i)
            {
                output.Write(transitions[i]);
            }
        }
    }

    public class TransitionsBuilderOld
    {
        private readonly int width;
        private readonly List<int[]> list = new();

        public TransitionsBuilderOld(int width)
        {
            this.width = width;
        }

        public static TransitionsBuilderOld Create(int width)
            => new(width);

        public TransitionsBuilderOld Add(int[] transitions)
        {
            if (width != transitions.Length)
                throw new CompilerException(CGR.SC004_InvalidTransitionsLength, width, transitions.Length);

            list.Add(transitions);

            return this;
        }

        public TransitionsOld Build()
            => new(width, list.ToArray());
    }
}
