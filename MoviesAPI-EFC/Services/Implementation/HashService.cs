using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MoviesAPI_EFC.DTOs.Security;
using System.Security.Cryptography;

namespace MoviesAPI_EFC.Services.Implementation
{
    public class HashService
    {
        public HashResponse GetHash(string plainText)
        {
            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return GetHash(plainText, salt);
        }

        private HashResponse GetHash(string plainText, byte[] salt)
        {
            var derivedKey = KeyDerivation.Pbkdf2(password: plainText, salt: salt, prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 32);
            var hash = Convert.ToBase64String(derivedKey);
            return new HashResponse() { Hash = hash, Salt = salt };
        }
    }
}
