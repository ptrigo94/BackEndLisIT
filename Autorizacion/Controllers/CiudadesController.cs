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
    public class CiudadesController : Controller
    {
        private readonly AuthDbContext _localizacionDbContext;
        public CiudadesController(AuthDbContext localizacionDbContext)
        {
            _localizacionDbContext = localizacionDbContext;
        }
        [HttpGet]
        public async Task <IActionResult> GetCiudades()
        {
            var ciudades = await _localizacionDbContext.Ciudades.ToListAsync();
            return Ok(ciudades);
        }
        [HttpGet("{id}")]
        public async Task <IActionResult> GetCiudad(int id)
        {
            var ciudad = await _localizacionDbContext.Ciudades.FindAsync(id);
            return Ok(ciudad);
        }



        //[Authorize(Roles = "Administrador")]
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCiudad([FromBody] Ciudad ciudad)
        {
            if (ciudad == null)
            {
                return BadRequest("Los datos enviados no son válidos.");
            }

            _localizacionDbContext.Ciudades.Add(ciudad);
            await _localizacionDbContext.SaveChangesAsync();

            return CreatedAtAction("GetRegion", new { id = ciudad.CiudadId }, ciudad);
        }


        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] Ciudad ciudad)
        {
            var ciudadAModificar = await _localizacionDbContext.Ciudades.FindAsync(id);
            if (ciudad == null)
            {
                return BadRequest("Los datos proporcionados son incorrectos");
            }

            if (ciudadAModificar == null)
            {
                return NotFound("Ciudad no encontrada.");
            }



            ciudadAModificar.Nombre = ciudad.Nombre;
            ciudadAModificar.RegionId = ciudad.RegionId;

            _localizacionDbContext.Ciudades.Update(ciudadAModificar);
            await _localizacionDbContext.SaveChangesAsync();
            return Ok("Actualizado con Exito");
        }

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            var ciudadAEliminar = await _localizacionDbContext.Ciudades.FindAsync(id);
            if (ciudadAEliminar == null)
            {
                return NotFound("Región no encontrada.");
            }
            _localizacionDbContext.Ciudades.Remove(ciudadAEliminar);
            await _localizacionDbContext.SaveChangesAsync();

            return Ok("Eliminado con Exito");
        }

    }
}
