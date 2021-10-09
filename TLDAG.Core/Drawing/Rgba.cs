using static System.FormattableString;
using static System.Math;

namespace TLDAG.Core.Drawing
{
    public readonly struct Rgba
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public Rgba(float r, float g, float b, float a)
        {
            R = Max(0, Min(1, r));
            G = Max(0, Min(1, g));
            B = Max(0, Min(1, b));
            A = Max(0, Min(1, a));
        }

        public override string ToString() => Invariant($"rgba({R:#0.##}, {G:#0.##}, {B:#0.##}, {A:#0.##})");
    }
}
