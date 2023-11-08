using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autorizacion.Data;
using AyudasSociales.Models;
using AyudasSociales.Data;
using Microsoft.AspNetCore.Authorization;

namespace AyudasSociales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AyudaRegionesController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly AuthDbContext _localizacionDbContext;
        public AyudaRegionesController(AuthDbContext context)
        {
            _context = context;
            _localizacionDbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AyudaRegion>>> GetAyudaRegion()
        {
            return await _context.AyudaRegion.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AyudaRegion>> GetAyudaRegion(int id)
        {
            var ayudaRegion = await _context.AyudaRegion.FindAsync(id);

            if (ayudaRegion == null)
            {
                return NotFound();
            }

            return ayudaRegion;
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAyudaRegion(int id, AyudaRegion ayudaRegion)
        {
            if (id != ayudaRegion.Id)
            {
                return BadRequest();
            }

            _context.Entry(ayudaRegion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AyudaRegionExists(id))
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
        public async Task<ActionResult<AyudaRegionDTO>> PostAyudaRegion(AyudaRegionDTO ayudaRegion)
        {
            var regionId = ayudaRegion.RegionId;
            List<Ciudad> comunas =  await _localizacionDbContext.Ciudades.Where(c => c.RegionId == regionId).ToListAsync();
           
            foreach (var comuna in comunas)
            {
                AyudaComuna ayudaComuna = new AyudaComuna();
                ayudaComuna.AyudaSocialId = ayudaRegion.AyudaSocialId;
                ayudaComuna.CiudadId = comuna.CiudadId;
                _context.AyudaComuna.Add(ayudaComuna);
                await _context.SaveChangesAsync();

                Console.WriteLine("-----" + comuna.Nombre + "-----");
            }
            AyudaRegion aRegion = new AyudaRegion();
            aRegion.RegionId = regionId;
            aRegion.AyudaSocialId = ayudaRegion.AyudaSocialId;
            _context.AyudaRegion.Add(aRegion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAyudaRegion", new { id = aRegion.Id }, aRegion);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAyudaRegion(int id)
        {
            var ayudaRegion = await _context.AyudaRegion.FindAsync(id);
            if (ayudaRegion == null)
            {
                return NotFound();
            }

            _context.AyudaRegion.Remove(ayudaRegion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AyudaRegionExists(int id)
        {
            return _context.AyudaRegion.Any(e => e.Id == id);
        }
    }
}
