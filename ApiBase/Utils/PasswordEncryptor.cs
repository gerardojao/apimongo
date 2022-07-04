using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace ApiBase.Utils
{
    public class PasswordEncryptor
    {
        private readonly IConfiguration configuration;
        private readonly RNGCryptoServiceProvider randomNumberGenerator;

        public PasswordEncryptor(IConfiguration _configuration)
        {
            configuration = _configuration;
            
            randomNumberGenerator = new RNGCryptoServiceProvider();
        }

        // Encrypts password using SHA256 algorithm.
        public string Encrypt(string password)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + configuration["Encryption:Key"]));  
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();  
            return hash;   
        }

        public bool Compare(string passwordAttempt, string hashedPassword)
        {
            var watch = Encrypt(passwordAttempt + configuration["Encryption:Key"]);
            return (Encrypt(passwordAttempt) == hashedPassword);
        }
    }
}