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

namespace AyudasSociales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AyudasSocialesController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public AyudasSocialesController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: api/AyudasSociales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AyudaSocial>>> GetAyudasSociales()
        {
            return await _context.AyudasSociales.ToListAsync();
        }

        // GET: api/AyudasSociales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AyudaSocial>> GetAyudaSocial(int id)
        {
            var ayudaSocial = await _context.AyudasSociales.FindAsync(id);

            if (ayudaSocial == null)
            {
                return NotFound();
            }

            return ayudaSocial;
        }

        // PUT: api/AyudasSociales/5
         
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAyudaSocial(int id, AyudaSocialDTO ayudaSocial)
        {
           

            _context.Entry(ayudaSocial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AyudaSocialExists(id))
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


        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<AyudaSocialDTO>> PostAyudaSocial([FromBody]AyudaSocialDTO ayudaSocial)
        {
            AyudaSocial aSocial  = new AyudaSocial();
            aSocial.Nombre = ayudaSocial.Nombre;
            aSocial.Descripcion = ayudaSocial.Descripcion;
            aSocial.Anio = ayudaSocial.Anio;
            aSocial.Vigente= ayudaSocial.Vigente;



            _context.AyudasSociales.Add(aSocial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAyudaSocial", new { id = aSocial.Id }, aSocial);
        }

        // DELETE: api/AyudasSociales/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAyudaSocial(int id)
        {
            var ayudaSocial = await _context.AyudasSociales.FindAsync(id);
            if (ayudaSocial == null)
            {
                return NotFound();
            }

            _context.AyudasSociales.Remove(ayudaSocial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AyudaSocialExists(int id)
        {
            return _context.AyudasSociales.Any(e => e.Id == id);
        }
    }
}
