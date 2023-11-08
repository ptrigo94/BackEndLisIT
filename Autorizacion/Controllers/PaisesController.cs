
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AyudasSociales.Data;
using AyudasSociales.Models;
using Autorizacion.Data;
using Microsoft.AspNetCore.Authorization;

namespace AyudasSociales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisesController : Controller
    {


        private readonly AuthDbContext _localizacionDbContext;
        public PaisesController(AuthDbContext localizacionDbContext)
        {
            _localizacionDbContext = localizacionDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaises()
        {
            var paises = await _localizacionDbContext.Paises.ToListAsync();
            return Ok(paises);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPais(int id)
        {
            var pais = await _localizacionDbContext.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound(); // Devuelve un 404 Not Found si el país no se encuentra
            }

            return Ok(pais);
        }
        [HttpGet("{id}/regiones")]
        public async Task<IActionResult> GetRegiones(int id)
        {
            var regiones = await _localizacionDbContext.Regiones.Where(r => r.PaisId == id).ToListAsync();
            Console.WriteLine(regiones);
            if (regiones.Count <= 0)
            {
                return NotFound();
            }
            return Ok(regiones);
        }


       // [Authorize(Roles = "Administrador")]
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePais([FromBody] Pais pais)
        {
            if (pais == null)
            {
                return BadRequest("Los datos enviados no coinciden.");
            }

            _localizacionDbContext.Paises.Add(pais);
            await _localizacionDbContext.SaveChangesAsync();

            return CreatedAtAction("GetPais", new { id = pais.Id }, pais);
        }
        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePais(int id , [FromBody] Pais pais)
        {
            var paisAModificar = await _localizacionDbContext.Paises.FindAsync(id);
            if (pais == null)
            {
                return BadRequest("Los datos proporcionados son incorrectos");
            }

            if (paisAModificar == null)
            {
                return NotFound("País no encontrado.");
            }



            paisAModificar.Nombre = pais.Nombre;
            paisAModificar.Siglas = pais.Siglas;

            _localizacionDbContext.Paises.Update(paisAModificar);
            await _localizacionDbContext.SaveChangesAsync();
            return Ok("Actualizado con Exito");
        }
        //[Authorize (Roles = "Administrador")]
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task <IActionResult> DeletePais (int id)
        {
            var paisAEliminar = await _localizacionDbContext.Paises.FindAsync(id);
            if (paisAEliminar == null)
            {
                return NotFound("País no encontrado.");
            }
            _localizacionDbContext.Paises.Remove(paisAEliminar);
            await _localizacionDbContext.SaveChangesAsync();

            return Ok("Eliminado con Exito");
        }
    }
}
