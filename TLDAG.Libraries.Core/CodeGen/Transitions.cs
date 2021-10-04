using System.Collections.Generic;
using System.IO;
using TLDAG.Libraries.Core.IO;
using TLDAG.Libraries.Core.Resources;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class Transitions
    {
        private readonly int width;
        private readonly int[][] transitions;

        internal Transitions(int width, int[][] transitions)
        {
            this.width = width;
            this.transitions = transitions;
        }

        public int this[int state, int symbol]
        {
            get => transitions[state][symbol];
        }

        public static Transitions Load(Stream stream)
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

    public class TransitionsBuilder
    {
        private readonly int width;
        private readonly List<int[]> list = new();

        public TransitionsBuilder(int width)
        {
            this.width = width;
        }

        public static TransitionsBuilder Create(int width)
            => new(width);

        public TransitionsBuilder Add(int[] transitions)
        {
            if (width != transitions.Length)
                throw new CompilerException(CGR.SC004_InvalidTransitionsLength, width, transitions.Length);

            list.Add(transitions);

            return this;
        }

        public Transitions Build()
            => new(width, list.ToArray());
    }
}
