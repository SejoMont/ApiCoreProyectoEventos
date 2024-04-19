using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreProyectoSejo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private EventosRepository repo;

        public EventosController(EventosRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventos()
        {
            var eventos = await repo.GetAllEventosAsync();

            return Ok(eventos);
        }

        [HttpGet("GetEvento/{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            var evento = await repo.GetDetallesEventoAsync(id);
            if (evento == null)
                return NotFound();

            return Ok(evento);
        }

        [HttpPost("CrearEvento")]
        public async Task<IActionResult> CrearEvento([FromForm] Evento evento, IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var filePath = await SaveFile(imagen);
                    evento.Imagen = filePath;
                    await repo.CrearEventoAsync(evento);
                    return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoID }, evento);
                }
                else
                {
                    return BadRequest("La imagen es requerida.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("AddComentario")]
        public async Task<IActionResult> AddComentario([FromBody] Comentario comentario)
        {
            await repo.AddComentarioAsync(comentario);

            return CreatedAtAction(nameof(GetEvento), new { id = comentario.EventoID });
        }

        // Helper method to save a file
        private async Task<string> SaveFile(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
