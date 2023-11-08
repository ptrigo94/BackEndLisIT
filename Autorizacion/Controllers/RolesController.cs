using Autorizacion.Data;
using Autorizacion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autorizacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {

        private readonly AuthDbContext _authDbContext;
        public RolesController(AuthDbContext autorizacionDbContext)
        {
            _authDbContext = autorizacionDbContext;

        }
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var rolesWithUsers = await _authDbContext.Roles
            .Include(r => r.Users)
            .ToListAsync();

            if (rolesWithUsers == null || !rolesWithUsers.Any())
            {
                return NotFound("No se ha encontrado la información solicitada.");
            }

            var result = rolesWithUsers.Select(role => new
            {
                RoleId = role.Id,
                RoleName = role.Nombre,
                Descripcion = role.Descripcion,
                Users = role.Users.Select(user => new
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email,

                })
            });

            return Ok(result);
        }
        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _authDbContext.Roles
                .Where(r => r.Id == id)
                .Include(r => r.Users)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return NotFound("No se ha encontrado la información solicitada.");
            }

            // Realiza una transformación de datos si es necesario, similar al ejemplo anterior.
            var result = new
            {
                RoleId = role.Id,
                RoleName = role.Nombre,
                Descripcion = role.Descripcion,
                Users = role.Users.Select(user => new
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email,
                })
            };

            return Ok(result);

        }

        [HttpPost] 
        public async Task<IActionResult> CreateRole([FromBody] Role rol)
        {
            if (rol == null)
            {
                return BadRequest("Los datos enviados no coinciden.");
            }
            _authDbContext.Roles.Add(rol);
            await _authDbContext.SaveChangesAsync();
            return CreatedAtAction("GetRole", new { id = rol.Id }, rol);
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var rol = await _authDbContext.Roles.FindAsync(id);
            if (rol == null) { return NotFound("No se ha encontrado la información solicitada."); }
            _authDbContext.Roles.Remove(rol);
            await _authDbContext.SaveChangesAsync();
            return Ok("Eliminado con exito");
        }
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        {
            var rolAModificar = await _authDbContext.Roles.FindAsync(id);
            if (role == null)
            {
                return BadRequest("Los datos proporcionados son incorrectos");
            }

            if (rolAModificar == null)
            {
                return NotFound("País no encontrado.");
            }



            rolAModificar.Nombre = role.Nombre;
            rolAModificar.Descripcion = role.Descripcion;

            _authDbContext.Roles.Update(rolAModificar);
            await _authDbContext.SaveChangesAsync();
            return Ok("Actualizado con Exito");
        }



    }
}
