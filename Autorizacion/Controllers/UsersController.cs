using Autorizacion.Data;
using Autorizacion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;

namespace Autorizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _iconfig;
        public UsersController(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _iconfig = configuration;
        }

        // GET: api/Users
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();

            // Proyecta los resultados para incluir el objeto Role en la respuesta JSON
            var usersWithRole = users.Select(user => new
            {
                user.Id,
                user.Username,
                user.PasswordHash,
                user.PasswordSalt,
                user.Email,
                user.Nombre,
                user.Apellido,
                user.RoleId,
                Role = new
                {
                    user.Role.Id,
                    user.Role.Nombre,
                    user.Role.Descripcion,
                    // Otras propiedades de Role si es necesario
                }
            });

            return Ok(usersWithRole);
        }

        // GET: api/Users/5
        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> PostUser(UserDTO newUser)
        {
            // busqueda email
            var existeUsuario = await _context.Users.Where(u => u.Email.Trim().Equals(newUser.Email.Trim())).FirstOrDefaultAsync();
            if (existeUsuario != null)
            {
                return BadRequest("Ya existe un usuario registrado con el correo proporcionado.");
            }

            CreatePasswordHash(newUser.Password, out byte[] passwordSalt, out byte[] PasswordHash);
            User user = new User();
            user.Username = newUser.UserName;
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = PasswordHash;
            user.Email = newUser.Email;
            user.Nombre = newUser.Nombre;
            user.Apellido = newUser.Apellido;
            user.RoleId = newUser.RoleId;
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }


    }
}
