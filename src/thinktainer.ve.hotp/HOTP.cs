namespace Thinktainer.Ve.HOTP
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    internal static class HOTP
    {
        internal static string Generate(string userId, long counter)
        {
            using (var generator = new HMACSHA1 { Key = Encoding.UTF8.GetBytes(userId) })
            {
                var computedHash = generator.ComputeHash(BitConverter.GetBytes(counter));
                return Transform(computedHash).ToString("D6");
            }
        }

        private static int Transform(IReadOnlyList<byte> computedHash)
        {
            var offset = computedHash[19] & 0xf; // low order 4 bits of last byte
            // extract the 4 bytes starting at offset
            int binCode = (computedHash[offset] & 0x7f) << 24 // & 0x7f masks most significant bit of returned byte code
                          | (computedHash[offset + 1] & 0xff) << 16
                          | (computedHash[offset + 2] & 0xff) << 8
                          | (computedHash[offset + 3] & 0xff);
            return binCode % (int)Math.Pow(10, 6);
        }
    }
}
