using static System.Math;

namespace TLDAG.Core.Algorithms
{
    public static class Maths
    {
        public static int Mod(int value, int min, int max)
        {
            int realMin = Min(min, max), realMax = Max(min, max);
            int modulus = realMax - realMin + 1;

            return realMin + Abs(value % modulus);
        }
    }
}
