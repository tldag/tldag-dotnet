using System.Security.Cryptography;

namespace TLDAG.Core.Cryptography
{
    public static class Keys
    {
        public static byte[] NewRsaKeyPair(int keySize = 1024) => new RSACryptoServiceProvider(keySize).ExportCspBlob(true);
    }
}
