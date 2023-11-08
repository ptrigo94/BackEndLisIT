using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autorizacion.Data;
using AyudasSociales.Models;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace AyudasSociales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AyudaComunasController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public AyudaComunasController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: api/AyudaComunas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AyudaComuna>>> GetAyudaComuna()
        {
            return await _context.AyudaComuna.ToListAsync();
        }

        // GET: api/AyudaComunas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AyudaComuna>> GetAyudaComuna(int id)
        {
            var ayudaComuna = await _context.AyudaComuna.FindAsync(id);

            if (ayudaComuna == null)
            {
                return NotFound();
            }

            return ayudaComuna;
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAyudaComuna(int id, AyudaComuna ayudaComuna)
        {
            if (id != ayudaComuna.Id)
            {
                return BadRequest();
            }

            _context.Entry(ayudaComuna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AyudaComunaExists(id))
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

        // POST: api/AyudaComunas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<AyudaComunaDTO>> PostAyudaComuna([FromBody] AyudaComunaDTO ayudaComuna)
        {
            AyudaComuna aComuna = new AyudaComuna();
            aComuna.CiudadId = ayudaComuna.CiudadId;
            aComuna.AyudaSocialId = ayudaComuna.AyudaSocialId;
            _context.AyudaComuna.Add(aComuna);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAyudaComuna", new { id = aComuna.Id }, aComuna);
        }

        // DELETE: api/AyudaComunas/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAyudaComuna(int id)
        {
            var ayudaComuna = await _context.AyudaComuna.FindAsync(id);
            if (ayudaComuna == null)
            {
                return NotFound();
            }

            _context.AyudaComuna.Remove(ayudaComuna);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AyudaComunaExists(int id)
        {
            return _context.AyudaComuna.Any(e => e.Id == id);
        }
    }
}
