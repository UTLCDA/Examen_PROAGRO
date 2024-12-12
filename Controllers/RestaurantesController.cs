using Microsoft.AspNetCore.Mvc;
using Examen_PROAGRO.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Examen_PROAGRO.Controllers
{
    public class RestaurantesController : Controller
    {
        private readonly ProagrodbContext _context; // nuevo

        // Este es el único constructor que debe tener el controlador
        public RestaurantesController(ProagrodbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Recuperar todos los restaurantes de la base de datos
            var restaurantes = await _context.Restaurantes.ToListAsync();

            // Pasar los restaurantes a la vista
            return View(restaurantes);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMarker([FromBody] Restaurante markerData)
        {
            if (markerData == null)
            {
                return BadRequest("Datos inválidos.");
            }

            // Verificar si ya existe un restaurante con el mismo FsqId
            var existingRestaurante = await _context.Restaurantes
                .FirstOrDefaultAsync(r => r.FsqId == markerData.FsqId);

            if (existingRestaurante != null)
            {
                return Conflict(new { message = $"El restaurante : {existingRestaurante.Nombre} ya existe en tu lista de FAVORITOS" });
            }

            var restaurante = new Restaurante
            {
                FsqId = markerData.FsqId,
                Nombre = markerData.Nombre,
                FormattedAddress = markerData.FormattedAddress,
                FechaRegistro = markerData.FechaRegistro,
                Activo = markerData.Activo,
                Foto = markerData.Foto,
                Rating = markerData.Rating
            };

            // Agregar el nuevo restaurante a la base de datos
            await _context.Restaurantes.AddAsync(restaurante);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Se añadio correctamente : {restaurante.Nombre} a tu lista de FAVORITOS." });
        }

        public async Task<IActionResult> Editar(int id)
        {
            var restaurante = await _context.Restaurantes.FindAsync(id);

            if (restaurante == null)
            {
                return NotFound();
            }

            return View(restaurante);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(string id, Restaurante restaurante)
        {
            if (id != restaurante.FsqId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestauranteExists(restaurante.FsqId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); 
            }
            return View(restaurante);
        }

        private bool RestauranteExists(string id)
        {
            return _context.Restaurantes.Any(e => e.FsqId == id);
        }
    }
}
