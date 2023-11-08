using Autorizacion.Data;
using AyudasSociales.Data;
using AyudasSociales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AyudasSociales.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RegionesController : Controller
    {

        private readonly AuthDbContext _localizacionDbContext;
        public RegionesController(AuthDbContext localizacionDbContext)
        {
            _localizacionDbContext = localizacionDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegiones()
        {
            var regiones = await _localizacionDbContext.Regiones.ToListAsync();
            return Ok(regiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegion(int id)
        {

            var region = await _localizacionDbContext.Regiones.FindAsync(id);

            if (region == null)
            {
                return NotFound(); 
            }

            return Ok(region);
        }
        [HttpGet("{id}/ciudades")]
        public async Task<IActionResult> GetCiudadesPorRegiones(int id)
        {
            var ciudades = await _localizacionDbContext.Ciudades.Where(c => c.RegionId == id).ToListAsync();
            if (ciudades.Count >0)
            {
                return Ok(ciudades);
                
            }
            return NotFound();
        }



        //[Authorize(Roles = "Administrador")]
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRegion([FromBody] Region region)
        {
            if (region == null)
            {
                return BadRequest("Los datos enviados no coinciden.");
            }

            _localizacionDbContext.Regiones.Add(region);
            await _localizacionDbContext.SaveChangesAsync();

            return CreatedAtAction("GetRegion", new { id = region.RegionId }, region);
        }


        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] Region region)
        {
            var regionAModificar = await _localizacionDbContext.Regiones.FindAsync(id);
            if (region == null)
            {
                return BadRequest("Los datos proporcionados son incorrectos");
            }

            if (regionAModificar == null)
            {
                return NotFound("Región no encontrada.");
            }



            regionAModificar.Name = region.Name;
            regionAModificar.Siglas = region.Siglas;
            regionAModificar.PaisId = region.PaisId;

            _localizacionDbContext.Regiones.Update(regionAModificar);
            await _localizacionDbContext.SaveChangesAsync();
            return Ok("Actualizado con Exito");
        }
        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var regionAEliminar = await _localizacionDbContext.Regiones.FindAsync(id);
            if (regionAEliminar == null)
            {
                return NotFound("Región no encontrada.");
            }
            _localizacionDbContext.Regiones.Remove(regionAEliminar);
            await _localizacionDbContext.SaveChangesAsync();

            return Ok("Eliminado con Exito");
        }

    }
}
