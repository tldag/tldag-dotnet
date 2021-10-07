using System;
using System.Linq;
using TLDAG.Core.Collections;
using static System.Math;

namespace TLDAG.Core.Cryptography
{
    public static class Passwords
    {
        private static readonly CharSet Upper = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        private static readonly CharSet Lower = new("abcdefghijklmnopqrstuvwxyz");
        private static readonly CharSet Digits = new("0123456789");
        private static readonly CharSet Valids = Upper + Lower + Digits;

        public static string NewPassword(int length)
        {
            length = Max(8, Min(int.MaxValue / 4, length));
            
            while (true)
            {
                byte[] bytes = Randoms.Bytes(length * 2);
                string password = ToPassword(bytes, length);

                if (IsStrong(password, length)) return password;
            }
        }

        private static string ToPassword(byte[] bytes, int length)
        {
            string base64 = Convert.ToBase64String(bytes);
            string password = new(base64.Where(c => Valids.Contains(c)).ToArray());

            if (password.Length < length) return password;

            for (int i = Randoms.NextInt(0, 16); i < length; i += Randoms.NextInt(0, 16))
            { password = password.Substring(0, i) + "-" + password.Substring(i); }

            return password.Substring(0, length);
        }

        private static bool IsStrong(string password, int length)
        {
            if (password.Length < length) return false;

            bool hasUpper = false, hasLower = false, hasDigit = false, hasDash = false;

            foreach (char c in password)
            {
                hasUpper = hasUpper || Upper.Contains(c);
                hasLower = hasLower || Lower.Contains(c);
                hasDigit = hasDigit || Digits.Contains(c);
                hasDash = password.IndexOf('-') >= 0;
            }

            return hasUpper && hasLower && hasDigit && hasDash;
        }
    }
}
