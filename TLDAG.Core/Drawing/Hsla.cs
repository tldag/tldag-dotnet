using static System.Math;

namespace TLDAG.Core.Drawing
{
    public readonly struct Hsla
    {
        public readonly float H;
        public readonly float S;
        public readonly float L;
        public readonly float A;

        public Hsla(float h, float s, float l, float a)
        {
            H = Max(0, Min(360, h));
            S = Max(0, Min(1, s));
            L = Max(0, Min(1, l));
            A = Max(0, Min(1, a));

            if (H == 360) H = 0;
        }
    }
}
