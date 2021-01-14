namespace Zaggoware.Games.Common
{
    using System;

    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    public static class GamePasswordHasher
    {
        private static byte[] Salt { get; set; } = Array.Empty<byte>();

        public static string HashPassword(string password)
        {
            var encryptedPassword = EncryptPassword(password);
            return Convert.ToBase64String(encryptedPassword);
        }

        public static bool VerifyPassword(string hash, string password)
        {
            var hashedPassword = HashPassword(password);
            return hash == hashedPassword;
        }

        private static byte[] EncryptPassword(string password)
        {
            return KeyDerivation.Pbkdf2(
                password,
                Salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8);
        }
    }
}