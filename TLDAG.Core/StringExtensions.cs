namespace TLDAG.Core
{
    public static class StringExtensions
    {
        public static bool IsDigit(this char c)
        {
            if (c == '0') return true;
            if (c == '1') return true;
            if (c == '2') return true;
            if (c == '3') return true;
            if (c == '4') return true;
            if (c == '5') return true;
            if (c == '6') return true;
            if (c == '7') return true;
            if (c == '8') return true;
            if (c == '9') return true;

            return false;
        }

        public static bool IsDigits(this string s)
        {
            foreach (char c in s)
            {
                if (!c.IsDigit()) return false;
            }

            return true;
        }
    }
}
