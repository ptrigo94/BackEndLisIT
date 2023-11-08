using Autorizacion.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Autorizacion.Utils
{
    public class AuthUtils
    {

        private readonly IConfiguration _iconfig;
        public AuthUtils(IConfiguration configuration)
        {


            _iconfig = configuration;
        }
        public void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }




        public bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            Console.WriteLine("------------" + passwordSalt);
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                Console.WriteLine("------------" + hmac + "---------------");
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }




        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre ),
                new Claim (ClaimTypes.Role, user.Role.Nombre)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_iconfig.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
