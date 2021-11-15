using System.IO;
using System.Security.Cryptography;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.Cryptography
{
    public static class Cryptor
    {
        public static byte[] Encrypt(byte[] plain, string password)
        {
            using TripleDES des = GetTripleDES(password);
            using MemoryStream input = new(plain);
            using MemoryStream output = new();

            WriteCount(plain.Length, output);

            using CryptoStream cs = GetCryptoStream(des, output, CryptoStreamMode.Write);

            input.CopyTo(cs); cs.Flush(); cs.Close();

            return output.ToArray();
        }

        public static byte[] Decrypt(byte[] encrypted, string password)
        {
            using TripleDES des = GetTripleDES(password);
            using MemoryStream input = new(encrypted);
            int count = ReadCount(input);
            using MemoryStream output = new();
            using CryptoStream cs = GetCryptoStream(des, input, CryptoStreamMode.Read);

            cs.CopyTo(output); cs.Close();

            return Resize(output.ToArray(), count);
        }

        private static CryptoStream GetCryptoStream(TripleDES des, Stream stream, CryptoStreamMode mode)
        {
            return new(stream, GetTransform(des, mode), mode);
        }

        private static ICryptoTransform GetTransform(TripleDES des, CryptoStreamMode mode)
        {
            if (mode == CryptoStreamMode.Read) return des.CreateDecryptor(des.Key, des.IV);
            else return des.CreateEncryptor(des.Key, des.IV);
        }

        private static TripleDES GetTripleDES(string password)
        {
            TripleDES des = TripleDES.Create();

            des.IV = new byte[des.BlockSize / 8];
            des.Key = Passwords.ToKey(password, des.KeySize);
            des.Padding = PaddingMode.Zeros;

            return des;
        }

        private static int ReadCount(Stream stream)
        {
            byte[] bytes = new byte[sizeof(int)];

            stream.Read(bytes, 0, bytes.Length);

            return BytesToInt(bytes);
        }

        private static void WriteCount(int count, Stream stream)
        {
            byte[] bytes = IntToBytes(count);

            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
