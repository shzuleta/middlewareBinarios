using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace FBapiService.Models.Security
{
    public class FBJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public FBJwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        // Método para generar el token
        public string GenerateToken(string username, string role, DateTime expiration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Role, role),
                //new Claim("Expiration", expirationMinutes.ToString()),
             };

            //token
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: expiration, signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);

       }

        public static byte[] GenerateSecretKey()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[32]; // 32 bytes para 256 bits
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return randomBytes;
            }
        }

        // Método para generar una clave secreta segura de longitud 'length' en bytes
        public static string GenerateSecretKey1(int length)
        {
            // Definir caracteres válidos para la clave secreta
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var validCharsArray = validChars.ToCharArray();

            // Generar una clave aleatoria segura usando RNGCryptoServiceProvider
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                // Convertir los bytes aleatorios en una cadena de caracteres válidos
                char[] result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = validCharsArray[randomBytes[i] % validCharsArray.Length];
                }

                return new string(result);
            }
        }
    }
      

}
