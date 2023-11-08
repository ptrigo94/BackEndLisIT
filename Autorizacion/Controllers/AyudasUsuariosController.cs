using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autorizacion.Data;
using AyudasSociales.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AyudasSociales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AyudasUsuariosController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public AyudasUsuariosController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: api/AyudasUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AyudaUsuario>>> GetAyudasUsuarios()
        {
            return await _context.AyudasUsuarios.ToListAsync();
        }

        // GET: api/AyudasUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AyudaUsuario>> GetAyudaUsuario(int id)
        {
            var ayudaUsuario = await _context.AyudasUsuarios.FindAsync(id);

            if (ayudaUsuario == null)
            {
                return NotFound();
            }

            return ayudaUsuario;
        }

        // PUT: api/AyudasUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAyudaUsuario(int id, AyudaUsuario ayudaUsuario)
        {
            if (id != ayudaUsuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(ayudaUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AyudaUsuarioExists(id))
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

        // POST: api/AyudasUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<AyudaUsuarioDTO>> PostAyudaUsuario([FromBody] AyudaUsuarioDTO ayudaUsuario)
        {
            if (AyudaUsuarioExists(ayudaUsuario.UserId))
            {
                return BadRequest("El usuario ya tiene un beneficio asignado.");
            }
            AyudaUsuario aUsuario = new AyudaUsuario();
            aUsuario.UserId = ayudaUsuario.UserId;
            aUsuario.AyudaSocialId = ayudaUsuario.AyudaSocialId;

            _context.AyudasUsuarios.Add(aUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAyudaUsuario", new { id = aUsuario.Id }, aUsuario);
        }

        // DELETE: api/AyudasUsuarios/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAyudaUsuario(int id)
        {
            var ayudaUsuario = await _context.AyudasUsuarios.FindAsync(id);
            if (ayudaUsuario == null)
            {
                return NotFound();
            }

            _context.AyudasUsuarios.Remove(ayudaUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AyudaUsuarioExists(int idUsuario)
        {
            return _context.AyudasUsuarios.Any(e => e.UserId == idUsuario);
        }
        [HttpGet("/usuario/{id}")]
        public async Task<ActionResult<IEnumerable<AyudaUsuario>>> GetAyudasUsuario(int id)
        {
            var ayudasUsuario = await _context.AyudasUsuarios
         .Where(a => a.UserId == id)
         .Select(a => new
         {
             UserId = a.UserId,
             AyudaId = a.AyudaSocialId,
             // Puedes agregar más propiedades según tus necesidades
             Ayuda = new
             {
                 Nombre=  a.Ayuda.Nombre,
                 Vigente = a.Ayuda.Vigente,
                 Descripcion = a.Ayuda.Descripcion,
                 Año =  a.Ayuda.Anio.Year
             },
             User = new
             {
                 Id = a.UserId,
                 Nombre = a.User.Nombre,
                 Apellido = a.User.Apellido,
                 Ciudad=  a.User.Ciudad.Nombre,


             }
         })
         .FirstOrDefaultAsync();

            if (ayudasUsuario == null)
            {
                return NotFound();
            }

            return Ok(ayudasUsuario);

        }
    }
}
