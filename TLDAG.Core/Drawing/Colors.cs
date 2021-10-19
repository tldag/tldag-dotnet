namespace TLDAG.Core.Drawing
{
    public static class Colors
    {
        private const float Epsilon = 0.001f;

        public static Rgba ToRgba(this Hsla hsla)
        {
            float rangedH = hsla.H / 360F;
            float r = 0, g = 0, b = 0;
            float s = hsla.S, l = hsla.L;

            if (l > Epsilon)
            {
                if (s < Epsilon)
                {
                    r = g = b = l;
                }
                else
                {
                    float temp2 = (l < .5F) ? l * (1F + s) : l + s - (l * s);
                    float temp1 = (2F * l) - temp2;

                    r = GetColorComponent(temp1, temp2, rangedH + 0.3333333F);
                    g = GetColorComponent(temp1, temp2, rangedH);
                    b = GetColorComponent(temp1, temp2, rangedH - 0.3333333F);
                }
            }

            return new(r, g, b, hsla.A);
        }

        private static float GetColorComponent(float first, float second, float third)
        {
            third = third < 0 ? third + 1 : (third > 1 ? third - 1 : third);

            if (third < 0.1666667F) return first + ((second - first) * 6F * third);
            if (third < .5F) return second;
            if (third < 0.6666667F) return first + ((second - first) * (0.6666667F - third) * 6F);
            return first;
        }
    }
}
