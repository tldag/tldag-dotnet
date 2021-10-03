using System.IO;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class ScannerData
    {
        public Alphabet Alphabet { get; }
        public Transitions Transitions { get; }
        public AcceptingStates Accepts { get; }

        public ScannerData(Alphabet alphabet, Transitions transitions, AcceptingStates accepts)
        {
            Alphabet = alphabet;
            Transitions = transitions;
            Accepts = accepts;
        }

        public static ScannerData Load(Stream stream)
        {
            Alphabet alphabet = Alphabet.Load(stream);
            Transitions transitions = Transitions.Load(stream);
            AcceptingStates accepts = AcceptingStates.Load(stream);

            return new(alphabet, transitions, accepts);
        }

        public void Save(Stream stream)
        {
            Alphabet.Save(stream);
            Transitions.Save(stream);
            Accepts.Save(stream);
        }
    }
}
