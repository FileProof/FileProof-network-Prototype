using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CVProof.Utils
{
    public static class RandomNonce
    {
        public static IEnumerable<byte> RandomBytes()
        {
            var random = new System.Random();
            byte[] buffer = new byte[32];
            while (true)
            {
                random.NextBytes(buffer);
                foreach (var ret in buffer)
                {
                    yield return ret;
                }
            }
        }

        public static IEnumerable<byte> CryptoRandomBytes()
        {
            var random = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[32];
            while (true)
            {
                random.GetBytes(buffer);
                foreach (var ret in buffer)
                {
                    yield return ret;
                }
            }
        }
    }
}
