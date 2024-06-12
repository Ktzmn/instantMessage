using System.Security.Cryptography;
using Data.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Core.Security
{
    public class EncryptionService
    {
        public Dictionary<string, string> GenerateHashWithSalt(string password, KeyDerivationPrf functionFamily, byte[]? saltExternal = null)
        {   
            var salt = RandomNumberGenerator.GetBytes(16);
            
            var derivedKey = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: saltExternal ?? salt,
                    prf: functionFamily,
                    iterationCount: 1000,
                    numBytesRequested: 32);

            return new Dictionary<string, string>
            {
                {"hashedPassword", Convert.ToBase64String(derivedKey)},
                {"salt", Convert.ToBase64String(saltExternal ?? salt)}
            };
        }

        public bool VerifyHashMatch(User user, string passwordProvided) 
        {
            if (user == null || passwordProvided == null)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            string hashResult = GenerateHashWithSalt(passwordProvided, KeyDerivationPrf.HMACSHA256, salt)["hashedPassword"];
    
            return string.Equals(
                user.Password,
                hashResult);
        }
    }
}