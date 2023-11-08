using Autorizacion.Data;
using Autorizacion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Autorizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _authDbContext;
        private readonly IConfiguration _iconfig;
        private readonly ILogger<AuthController> _logger;
        public AuthController(AuthDbContext autorizacionDbContext, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authDbContext = autorizacionDbContext;

            _iconfig = configuration;
            _logger = logger;
            _logger.LogDebug("NLog Integrado en AuthController");
        }

        public static User user = new User();

        //Registro usuario
        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(UserDTO request)
        {
            //busqueda email
            var existeUsuario = await _authDbContext.Users.Where(u => u.Email.Trim().Equals(request.Email.Trim())).FirstOrDefaultAsync();
            if (existeUsuario != null)
            {
                return BadRequest("Ya existe un usuario registrado con el correo proporcionado.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordSalt, out byte[] PasswordHash);
            user.Username = request.UserName;
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = PasswordHash;
            user.Email = request.Email;
            user.Nombre = request.Nombre;
            user.Apellido = request.Apellido;
            _authDbContext.Users.Add(user);
            _authDbContext.SaveChanges();
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            var existeUsuario = await _authDbContext.Users.Where(u => u.Email.Equals(request.Email)).FirstOrDefaultAsync();
            if (existeUsuario == null)
            {
                return BadRequest("No existe usuario registrado con el correo proporcionado.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordSalt, out byte[] PasswordHash);


            if (passwordSalt.SequenceEqual(existeUsuario.PasswordSalt) && PasswordHash.SequenceEqual(existeUsuario.PasswordHash))
            {
                return BadRequest("Contraseña erronea, intente nuevamente");
            }
            var rolID = existeUsuario.Id;


            Role role = await _authDbContext.Roles.Where(r => r.Id == rolID).FirstOrDefaultAsync();
            existeUsuario.Role = role;

            string token = CreateToken(existeUsuario);
            return Ok(token);
        }

        private Role GetRole(int id)
        {
            Role role = _authDbContext.Roles.Find(id);

            // Realiza una transformación de datos si es necesario, similar al ejemplo anterior.
            return role;
        }


        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }




        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            Console.WriteLine("------------" + passwordSalt);
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                Console.WriteLine("------------" + hmac + "---------------");
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }




        private string CreateToken(User user)
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
