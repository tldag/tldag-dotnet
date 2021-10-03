using System.IO;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class ScannerData
    {
        public Alphabet Alphabet { get; }
        public Transitions Transitions { get; }

        public ScannerData(Alphabet alphabet, Transitions transitions)
        {
            Alphabet = alphabet;
            Transitions = transitions;
        }

        public static ScannerData Load(Stream stream)
        {
            Alphabet alphabet = Alphabet.Load(stream);
            Transitions transitions = Transitions.Load(stream);

            return new(alphabet, transitions);
        }

        public void Save(Stream stream)
        {
            Alphabet.Save(stream);
            Transitions.Save(stream);
        }
    }
}
