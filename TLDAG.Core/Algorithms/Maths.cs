using static System.Math;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Algorithms
{
    public static class Maths
    {
        public static ushort Interpolate(ushort a, ushort b, double t) => (ushort)(a + Round((b - a) * t));
        public static int Interpolate(int a, int b, double t) => (int)(a + Round((b - a) * t));
        public static uint Interpolate(uint a, uint b, double t) => (uint)(a + Round((b - a) * t));
    }
}
