using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.IO;
using TLDAG.Libraries.Core.Resources;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class ScannerData
    {
        private Alphabet? alphabet = null;
        public Alphabet Alphabet { get => alphabet ?? throw new InvalidOperationException(CGR.SC001_AlphabetNotSet); }

        private int[][]? transitions = null;

        public ScannerData(Alphabet alphabet, int[][] transitions)
        {
            this.alphabet = alphabet;
            this.transitions = transitions;
        }

        public void Load(Stream stream)
        {
            alphabet = new();
            alphabet.Load(stream);

            LoadTransitions(stream);
        }

        public void Save(Stream stream)
        {
            Alphabet.Save(stream);
            SaveTransitions(stream);
        }

        private void LoadTransitions(Stream stream)
        {
            IntStream intStream = new(stream);
            int count = intStream.Read();
            int length = Alphabet.Count;

            transitions = new int[count][];

            for (int i = 0; i < count; ++i)
            {
                transitions[i] = new int[length];
                intStream.Read(transitions[i]);
            }
        }

        private void SaveTransitions(Stream stream)
        {
            if (transitions == null)
                throw new InvalidOperationException();

            IntStream intStream = new(stream);
            int count = transitions.Length;

            intStream.Write(count);

            for (int i = 0; i < count; ++i)
            {
                intStream.Write(transitions[i]);
            }
        }
    }
}
